package com.mst.services;

import com.fazecast.jSerialComm.SerialPort;
import com.mst.models.Company;
import com.mst.models.UserAccount;
import com.mst.repositories.ICompanyRepository;
import com.mst.repositories.WeighBridgeRepository;
import com.mst.security.CurrentUserContext;
import com.sun.jna.ptr.IntByReference;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import org.springframework.web.context.request.RequestContextHolder;
import org.springframework.web.context.request.ServletRequestAttributes;

import javax.annotation.PreDestroy;
import javax.servlet.http.HttpSession;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.UUID;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 * The weighbridge hardware, driven by the server — this application runs on the PC the indicator
 * and the cameras are wired to, exactly where the desktop form ran.
 *
 * <b>Indicator</b> — {@code frmWeightbridge.ReadWeightFromWeightBridge():4181}. The port settings
 * are {@code dtWbList.Rows[0]} (Sp_WeighbridgeScalesList_GetAllMethod): PortName, BaudRate,
 * ReadTimeOutWb, WriteTimeOutWb, DataBits, Parity, StopBits.One. The port is opened, the thread
 * sleeps 500 ms, {@code ReadExisting()} takes whatever the indicator has sent, the port is closed,
 * and the text is parsed by ModelName. jSerialComm stands in for System.IO.Ports.SerialPort.
 *
 * <b>Cameras</b> — {@code HVUtility} over the Hikvision SDK: NET_DVR_Init, NET_DVR_Login_V30,
 * NET_DVR_CaptureJPEGPicture (channel 1, wPicSize 255, wPicQuality 0). Three cameras, from the
 * configurations IpCameraAddress0N / PortNo0N / UserNameCamer0N / PasswordCamer0N; everything only
 * when "CameraPictureBox" (the picture folder) is set, as LoadCamera() does. After a reading above 0
 * the three JPEGs are written to that folder as {@code WBTicket_<ticket>_<guid>.jpg}. The camera
 * passwords never leave the server.
 */
@Service
public class WeighBridgeDeviceService {

    private static final Logger LOG = LoggerFactory.getLogger(WeighBridgeDeviceService.class);

    static final String CFG_CAMERA_PATH = "CameraPictureBox";
    private static final String SESSION_CAPTURED = "WB_CAPTURED_PICTURES";
    private static final Pattern FILE_NAME = Pattern.compile("^[A-Za-z0-9_.\\-]+\\.(?i:jpg|jpeg|bmp|png)$");
    private static final Pattern MATRIX = Pattern.compile("^\\D*?((-?(\\d+(\\.\\d+)?))|(-?\\.\\d+)).*", Pattern.DOTALL);
    private static final Pattern CS_DECIMAL = Pattern.compile("^\\s*([+-]?)(\\d*)(?:\\.(\\d*))?\\s*$");

    /** The six picture columns and the camera (1..3) each is taken with. */
    public static final List<String> FIRST_FIELDS = Arrays.asList("FirstWtPicReading", "FirstWtUpperPic", "FirstWtFrontPic");
    public static final List<String> SECOND_FIELDS = Arrays.asList("SecondPicReading", "SecondWtUpperPic", "SecondWtFrontPic");

    @Autowired private WeighBridgeRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private ICompanyRepository companies;

    /** Full path of HCNetSDK.dll, or just "HCNetSDK" to take it from PATH / jna.library.path. */
    @Value("${weighbridge.hcnetsdk.path:HCNetSDK}")
    private String sdkPath;

    /** NET_DVR_SetLogToFile(3, "C:\\SdkLog\\", true) — the desktop's own log folder. */
    @Value("${weighbridge.hcnetsdk.log-dir:C:\\SdkLog\\}")
    private String sdkLogDir;

    /** How long a live-view frame is reused before the camera is asked again. */
    @Value("${weighbridge.camera.preview-cache-ms:800}")
    private long previewCacheMs;

    private final Object serialLock = new Object();
    private final Object sdkLock = new Object();
    private volatile HikvisionSdk sdk;
    private volatile String sdkError;
    private final Map<String, Integer> logins = new HashMap<>();              // ip|port|user -> lUserID
    private final Map<Integer, Object[]> previewCache = new HashMap<>();      // camera -> {time, bytes}

    // ====================================================================== indicator (F5)

    public static final class ReadRequest {
        public String ticketNo;          // txtTickectNo — used in the picture file names
        public boolean updateMode;       // UpdateMode (a saved ticket is open)
        public boolean approved;         // Approved
        public List<String> firstTags;   // PicFirstReading1stTime.Tag / picFirstUpper.Tag / PicVehicleFront1stTime.Tag
        public List<String> secondTags;  // PicSecondReading2ndtime.Tag / PicSecondUpper.Tag / PicSecondVehicle2ndTime.Tag
    }

    /** ReadWeightFromWeightBridge():4181 — reading, target box, and the pictures it took. */
    public Map<String, Object> readWeight(ReadRequest r) {
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> scales = repo.scalesList(u);
        if (scales.isEmpty()) throw new IllegalArgumentException("There is no row at position 0.");
        Map<String, Object> s = scales.get(0);

        String machineData = readPort(s);
        String reading = parse(machineData, str(WeighBridgeRepository.ci(s, "ModelName")), companyName(u));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("machineData", machineData);                // labelSerialPortData.Text
        out.put("reading", reading);                        // getdata.ToString()
        out.put("target", r.updateMode && !r.approved ? "second" : (!r.updateMode ? "first" : null));
        List<String> warnings = new ArrayList<>();
        Map<String, String> pictures = new LinkedHashMap<>();

        boolean positive = toDouble(reading) > 0d;
        String folder = cfg(u, CFG_CAMERA_PATH);
        if (positive && !folder.trim().isEmpty() && out.get("target") != null) {
            boolean second = "second".equals(out.get("target"));
            List<String> fields = second ? SECOND_FIELDS : FIRST_FIELDS;
            List<String> tags = second ? r.secondTags : r.firstTags;
            // DeleteCameraPics(...Tag) — only pictures this session took (a Tag is only ever set here)
            if (tags != null) for (String t : tags) deleteCaptured(folder, t);
            String ticket = digits(r.ticketNo);
            String sdkProblem = null;
            try { synchronized (sdkLock) { sdk(); } } catch (RuntimeException e) { sdkProblem = e.getMessage(); }
            // Without the SDK no camera can be reached at all (the desktop would not start); no
            // names are written for pictures that cannot exist.
            if (sdkProblem != null) warnings.add(sdkProblem);
            else for (int i = 0; i < 3; i++) {
                String name = String.format("%s_%s.jpg", "WBTicket_" + ticket, UUID.randomUUID());
                String err = captureToFile(u, i + 1, Paths.get(folder, name).toString());
                if (err != null) warnings.add(err);
                // The Tag is set whether or not the capture worked, as CreateJPG only shows a message.
                pictures.put(fields.get(i), name);
                remember(name);
            }
        }
        out.put("pictures", pictures);
        out.put("warnings", warnings);
        return out;
    }

    private String readPort(Map<String, Object> s) {
        String portName = str(WeighBridgeRepository.ci(s, "PortName")).trim();
        int baud = WeighBridgeRepository.intOf(WeighBridgeRepository.ci(s, "BaudRate"));
        int readTimeout = WeighBridgeRepository.intOf(WeighBridgeRepository.ci(s, "ReadTimeOutWb"));
        int writeTimeout = WeighBridgeRepository.intOf(WeighBridgeRepository.ci(s, "WriteTimeOutWb"));
        int dataBits = WeighBridgeRepository.intOf(WeighBridgeRepository.ci(s, "DataBits"));
        String parityText = str(WeighBridgeRepository.ci(s, "Parity"));
        int parity = "Even".equals(parityText) ? SerialPort.EVEN_PARITY
                   : "Odd".equals(parityText) ? SerialPort.ODD_PARITY
                   : SerialPort.NO_PARITY;                   // "None", and .NET's default otherwise

        synchronized (serialLock) {
            if (portName.isEmpty()) throw new IllegalArgumentException("The PortName cannot be empty.");
            SerialPort port;
            try {
                port = SerialPort.getCommPort(portName);
            } catch (Exception e) {
                throw new IllegalArgumentException("The port '" + portName + "' does not exist.");
            }
            port.setComPortParameters(baud > 0 ? baud : 9600, dataBits > 0 ? dataBits : 8, SerialPort.ONE_STOP_BIT, parity);
            port.setFlowControl(SerialPort.FLOW_CONTROL_DISABLED);
            port.setComPortTimeouts(SerialPort.TIMEOUT_NONBLOCKING, Math.max(readTimeout, 0), Math.max(writeTimeout, 0));
            if (!port.openPort()) {
                // serialPort1.Open() — the two .NET messages: missing port vs. port in use.
                boolean listed = false;
                for (SerialPort p : SerialPort.getCommPorts()) {
                    if (p.getSystemPortName().equalsIgnoreCase(portName)) { listed = true; break; }
                }
                throw new IllegalArgumentException(listed
                        ? "Access to the port '" + portName + "' is denied."
                        : "The port '" + portName + "' does not exist.");
            }
            try {
                port.clearDTR();                               // .NET SerialPort: DtrEnable = false
                port.clearRTS();                               //                 RtsEnable = false
                Thread.sleep(500);                             // Thread.Sleep(500)
                int n = port.bytesAvailable();
                if (n <= 0) return "";
                byte[] buf = new byte[n];
                int got = port.readBytes(buf, n);
                return ascii(buf, Math.max(got, 0));           // ReadExisting() with the default ASCIIEncoding
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
                throw new IllegalStateException("Reading the indicator was interrupted.");
            } finally {
                port.closePort();
            }
        }
    }

    /** ASCIIEncoding turns every byte above 0x7F into '?'. */
    private static String ascii(byte[] b, int len) {
        StringBuilder sb = new StringBuilder(len);
        for (int i = 0; i < len; i++) {
            int c = b[i] & 0xFF;
            sb.append(c < 0x80 ? (char) c : '?');
        }
        return sb.toString();
    }

    /** The ModelName branches of ReadWeightFromWeightBridge, in the desktop's order. */
    static String parse(String data, String model, String companyName) {
        String getdata = "0";
        int i;
        if ("Standard".equals(model)) {
            i = data.indexOf("KG");
            if (i > 6) getdata = csDecimal(substr(data, i - 6, 6));
        } else if ("XK3190".equals(model)) {
            i = data.indexOf("+");
            if (i > 0) getdata = csDecimal(substr(data, i + 1, 6));
        } else if ("NGA10075".equals(model)) {
            i = data.indexOf("+");
            if (i > 0) getdata = csDecimal(substr(data, i + 1, 7));
        } else if ("SC17D".equals(model)) {
            i = data.indexOf("+");
            if (i >= 0) getdata = csDecimal(substr(data, i + 1, 8));
        } else if ("SAKINA INDUSTRY".equals(companyName)) {
            i = data.indexOf("kg");
            if (i > 0) getdata = csDecimal(substr(data, i - 7, 6));
        } else if ("MatrixII".equals(model)) {
            Matcher mx = MATRIX.matcher(data);
            getdata = mx.matches() ? csDecimal(mx.group(1)) : "0";
        }
        return getdata;
    }

    /** C# String.Substring throws when the range runs outside the text. */
    private static String substr(String s, int start, int len) {
        if (start < 0) throw new IllegalArgumentException("StartIndex cannot be less than zero.\r\nParameter name: startIndex");
        if (len < 0 || start + len > s.length()) {
            throw new IllegalArgumentException("Index and length must refer to a location within the string.\r\nParameter name: length");
        }
        return s.substring(start, start + len);
    }

    /** Conversion.ToDecimal(text) then decimal.ToString(): leading zeros go, the digits after the
     *  point stay as sent ("012.50" → "12.50"); anything unparsable is 0. */
    static String csDecimal(String t) {
        Matcher m = CS_DECIMAL.matcher(t == null ? "" : t);
        if (!m.matches()) return "0";
        String whole = m.group(2) == null ? "" : m.group(2);
        String frac = m.group(3);
        if (whole.isEmpty() && (frac == null || frac.isEmpty())) return "0";
        String intPart = whole.replaceFirst("^0+(?=\\d)", "");
        if (intPart.isEmpty()) intPart = "0";
        String out = intPart + (frac != null && !frac.isEmpty() ? "." + frac : "");
        if ("-".equals(m.group(1)) && out.matches(".*[1-9].*")) out = "-" + out;
        return out;
    }

    // ============================================================================ cameras

    public static final class CameraStatus {
        public int camera;
        public String name;
        public boolean ok;
        public String message;
    }

    /**
     * LoadCamera() / CameraInitilization — log each configured camera in and report what the
     * desktop would have shown in a message box. {@code second} names them Camera 04..06, as the
     * second-weight set is named on the desktop (the same three cameras).
     */
    public Map<String, Object> initCameras(boolean second) {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        boolean configured = !cfg(u, CFG_CAMERA_PATH).trim().isEmpty();
        out.put("configured", configured);
        List<CameraStatus> list = new ArrayList<>();
        if (configured) {
            for (int n = 1; n <= 3; n++) {
                CameraStatus st = new CameraStatus();
                st.camera = n;
                st.name = "Camera 0" + (second ? n + 3 : n);
                String[] c = camera(u, n);
                if (c[0].trim().isEmpty() || WeighBridgeRepository.intOf(c[1]) == 0 || c[2].trim().isEmpty() || c[3].trim().isEmpty()) {
                    st.message = st.name + " configuration missing!";
                } else {
                    try {
                        login(c);
                        st.ok = true;
                    } catch (RuntimeException e) {
                        st.message = st.name + " connection failed.\n" + e.getMessage();
                    }
                }
                list.add(st);
            }
        }
        out.put("cameras", list);
        return out;
    }

    /** Live view: one JPEG from the camera, reused for a moment so several open pages do not
     *  hammer it. */
    public byte[] snapshot(int cameraNo) {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (cameraNo < 1 || cameraNo > 3) throw new IllegalArgumentException("Unknown camera");
        if (cfg(u, CFG_CAMERA_PATH).trim().isEmpty()) throw new IllegalArgumentException("Cameras are not configured (CameraPictureBox).");
        synchronized (previewCache) {
            Object[] hit = previewCache.get(cameraNo);
            if (hit != null && System.currentTimeMillis() - (Long) hit[0] < previewCacheMs) return (byte[]) hit[1];
        }
        String[] c = camera(u, cameraNo);
        byte[] jpeg;
        synchronized (sdkLock) {
            HikvisionSdk s = sdk();
            int userId = login(c);
            byte[] buf = new byte[2 * 1024 * 1024];
            IntByReference size = new IntByReference(0);
            boolean ok = s.NET_DVR_CaptureJPEGPicture_NEW(userId, 1, jpegPara(), buf, buf.length, size);
            if (!ok) {
                int err = s.NET_DVR_GetLastError();
                dropLogin(c);
                throw new IllegalStateException("NET_DVR_CaptureJPEGPicture_NEW failed, error code= " + err);
            }
            jpeg = Arrays.copyOf(buf, Math.max(0, Math.min(size.getValue(), buf.length)));
        }
        synchronized (previewCache) { previewCache.put(cameraNo, new Object[] { System.currentTimeMillis(), jpeg }); }
        return jpeg;
    }

    /** HVUtility.CreateJPG — null when the file was written, otherwise the desktop's message. */
    private String captureToFile(UserAccount u, int cameraNo, String path) {
        String[] c = camera(u, cameraNo);
        synchronized (sdkLock) {
            HikvisionSdk s;
            int userId;
            try {
                s = sdk();
                userId = login(c);
            } catch (RuntimeException e) {
                return "Camera 0" + cameraNo + ": " + e.getMessage();
            }
            if (s.NET_DVR_CaptureJPEGPicture(userId, 1, jpegPara(), path)) return null;
            int err = s.NET_DVR_GetLastError();
            // A dropped session is logged in again once before giving up.
            dropLogin(c);
            try {
                userId = login(c);
                if (s.NET_DVR_CaptureJPEGPicture(userId, 1, jpegPara(), path)) return null;
                err = s.NET_DVR_GetLastError();
            } catch (RuntimeException ignored) { }
            return "NET_DVR_CaptureJPEGPicture failed, error code= " + err;
        }
    }

    private static HikvisionSdk.NET_DVR_JPEGPARA jpegPara() {
        HikvisionSdk.NET_DVR_JPEGPARA p = new HikvisionSdk.NET_DVR_JPEGPARA();
        p.wPicQuality = 0;
        p.wPicSize = 255;
        return p;
    }

    /** HVUtility constructor — NET_DVR_Init once, and the desktop's SDK log setting. */
    private HikvisionSdk sdk() {
        HikvisionSdk s = sdk;
        if (s != null) return s;
        if (sdkError != null) throw new IllegalStateException(sdkError);
        try {
            s = HikvisionSdk.load(sdkPath);
        } catch (Throwable t) {
            String why = t.getMessage() == null ? t.getClass().getSimpleName() : t.getMessage().split("\\R", 2)[0];
            sdkError = "HCNetSDK could not be loaded from '" + sdkPath + "' (" + why + "). Put the Hikvision "
                     + "Device Network SDK for this Java's bitness on the server and set weighbridge.hcnetsdk.path.";
            LOG.error(sdkError);
            throw new IllegalStateException(sdkError);
        }
        if (!s.NET_DVR_Init()) throw new IllegalStateException("NET_DVR_Init error!");
        try { s.NET_DVR_SetLogToFile(3, sdkLogDir, true); } catch (Throwable ignored) { }
        sdk = s;
        return s;
    }

    /** HVUtility.ConnectCamera — NET_DVR_Login_V30, kept for reuse while it stays valid. */
    private int login(String[] c) {
        String key = c[0] + "|" + c[1] + "|" + c[2];
        synchronized (sdkLock) {
            Integer id = logins.get(key);
            if (id != null && id >= 0) return id;
            HikvisionSdk s = sdk();
            HikvisionSdk.NET_DVR_DEVICEINFO_V30 info = new HikvisionSdk.NET_DVR_DEVICEINFO_V30();
            int userId = s.NET_DVR_Login_V30(c[0], WeighBridgeRepository.intOf(c[1]), c[2], c[3], info);
            if (userId < 0) throw new IllegalStateException("NET_DVR_Login_V30 failed, error code= " + s.NET_DVR_GetLastError());
            logins.put(key, userId);
            return userId;
        }
    }

    private void dropLogin(String[] c) {
        String key = c[0] + "|" + c[1] + "|" + c[2];
        synchronized (sdkLock) {
            Integer id = logins.remove(key);
            if (id != null && id >= 0 && sdk != null) {
                try { sdk.NET_DVR_Logout(id); } catch (Throwable ignored) { }
            }
        }
    }

    /** HVUtility.Disconnect for every session, when the application stops. */
    @PreDestroy
    public void shutdown() {
        synchronized (sdkLock) {
            if (sdk == null) return;
            for (Integer id : logins.values()) {
                try { if (id != null && id >= 0) sdk.NET_DVR_Logout(id); } catch (Throwable ignored) { }
            }
            logins.clear();
            try { sdk.NET_DVR_Cleanup(); } catch (Throwable ignored) { }
        }
    }

    /** GetConfigurationsFromGlobalAndBindValuesInColumns():753 — ip, port, user, password. */
    private String[] camera(UserAccount u, int n) {
        return new String[] {
                cfg(u, "IpCameraAddress0" + n),
                String.valueOf(WeighBridgeRepository.intOf(cfg(u, "PortNo0" + n))),
                cfg(u, "UserNameCamer0" + n),
                cfg(u, "PasswordCamer0" + n)
        };
    }

    // =========================================================================== pictures

    /** The file behind one picture column of a saved ticket of this company, or null. */
    public byte[] ticketPicture(int ticketId, String field) throws IOException {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (!FIRST_FIELDS.contains(field) && !SECOND_FIELDS.contains(field)) throw new IllegalArgumentException("Unknown picture");
        Map<String, Object> row = repo.readById(ticketId);
        if (row == null) return null;
        Object company = WeighBridgeRepository.ci(row, "CompanyId");
        if (company == null || WeighBridgeRepository.intOf(company) != u.getCompanyId()) return null;
        return readPicture(u, str(WeighBridgeRepository.ci(row, field)));
    }

    /** A picture taken in this session and not saved yet. */
    public byte[] capturedPicture(String name) throws IOException {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (!captured().contains(name)) return null;
        return readPicture(u, name);
    }

    private byte[] readPicture(UserAccount u, String name) throws IOException {
        if (name == null || !FILE_NAME.matcher(name).matches()) return null;
        String folder = cfg(u, CFG_CAMERA_PATH);
        if (folder.trim().isEmpty()) return null;
        Path p = Paths.get(folder, name);                   // CameraPixBox1Path + "\\" + name
        if (!Files.isRegularFile(p)) return null;           // File.Exists — else the box stays empty
        return Files.readAllBytes(p);
    }

    /**
     * Whether a picture name may be written onto a ticket: empty, a picture this session took, or
     * the value the ticket already holds.
     */
    public boolean acceptablePictureName(String name, String storedValue) {
        if (name == null || name.isEmpty()) return true;
        if (name.equals(storedValue)) return true;
        return captured().contains(name);
    }

    /** DeleteCameraPics — five attempts, as written; only pictures this session took. */
    private void deleteCaptured(String folder, String name) {
        if (name == null || name.isEmpty() || !captured().contains(name) || !FILE_NAME.matcher(name).matches()) return;
        Path p = Paths.get(folder, name);
        if (!Files.exists(p)) return;
        for (int attempts = 0; attempts < 5; attempts++) {
            try {
                Files.delete(p);
                captured().remove(name);
                return;
            } catch (IOException e) {
                try { Thread.sleep(100); } catch (InterruptedException ie) { Thread.currentThread().interrupt(); return; }
            }
        }
    }

    @SuppressWarnings("unchecked")
    private Set<String> captured() {
        HttpSession session = session();
        if (session == null) return new HashSet<>();
        synchronized (session) {
            Object o = session.getAttribute(SESSION_CAPTURED);
            if (!(o instanceof Set)) {
                o = Collections.synchronizedSet(new HashSet<String>());
                session.setAttribute(SESSION_CAPTURED, o);
            }
            return (Set<String>) o;
        }
    }

    private void remember(String name) { captured().add(name); }

    private static HttpSession session() {
        try {
            ServletRequestAttributes a = (ServletRequestAttributes) RequestContextHolder.getRequestAttributes();
            return a == null ? null : a.getRequest().getSession(true);
        } catch (Exception e) {
            return null;
        }
    }

    // ============================================================================ helpers

    private String cfg(UserAccount u, String name) {
        try {
            return repo.config(u, name);
        } catch (Exception e) {
            LOG.warn("Configuration '{}' could not be read; treating as unset", name, e);
            return "";
        }
    }

    private String companyName(UserAccount u) {
        try {
            Company c = companies.findById(u.getCompanyId()).orElse(null);
            return c == null || c.getCompName() == null ? "" : c.getCompName();
        } catch (Exception e) {
            return "";
        }
    }

    private static String digits(String s) {
        return s == null ? "" : s.replaceAll("[^0-9]", "");
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o); }

    private static double toDouble(String s) {
        try { return Double.parseDouble(s); } catch (Exception e) { return 0d; }
    }
}
