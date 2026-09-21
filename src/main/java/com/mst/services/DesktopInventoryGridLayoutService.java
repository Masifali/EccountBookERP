package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryGridColumn;
import com.mst.repositories.DesktopInventoryGridLayoutRepository;
import com.mst.security.*;
import java.io.*;
import java.nio.file.*;
import java.util.*;
import javax.xml.XMLConstants;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.transform.*;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;
import org.springframework.transaction.support.*;
import org.w3c.dom.*;

/** The source CtrlGrdBar layout contract, initially registered for Define Item's history. */
@Service
public class DesktopInventoryGridLayoutService {
    private static final String FORM="InvDefrmAddItem", GRID="grdhistory";
    private static final List<String> KEYS=List.of("ItemCodeNew","ItemName","ItemNameOtherLingo","TypeDescription","CategoryDescription","ParentCategory","ClassGroupName","ItemStatus","StockAc","SaleAc","CgsAc","ItemWithVarient","MotherItem","IsCompany","IsThirdParty","EntryDate","EntryUserName","NoOfAttachments");
    private final DesktopInventoryGridLayoutRepository repo;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    private final Path root;
    private record LayoutSpec(String form,String grid,int screen,List<String> keys,List<String> hidden) {}
    private static LayoutSpec spec(String target){
        return switch(target){
            case "defined-items" -> new LayoutSpec(FORM,GRID,111,KEYS,List.of("Id","ItemCode","Select"));
            case "pos-items" -> new LayoutSpec("InvAddItemsPOS","grdhistoryForHistory",106,List.of("ItemCode","ItemName","ItemCategory","ItemType","StockAccount","SaleAccount","CgsAccount","UserName","NoOfAttachments"),List.of("Id","RecordNo"));
            case "lots" -> new LayoutSpec("AcfrmDefineLots","GridDefinelots",110,List.of("JobLotCode","JobLotDescription","AccountTitle","StartDate","EndDate","JobStatus","JobType","RefDocumentType","RefDocNo","IsCompany","IsThirdParty"),List.of("Id","JobTypeId","AccountId"));
            case "item_types" -> new LayoutSpec("InvDeffrmItemType","grdhistory",113,List.of("Code","Description","Type","ParentCategory","MotherStatus"),List.of("Id"));
            case "definition-categories" -> new LayoutSpec("InvDeffrmItemCatagory","grdfrm",112,List.of("CategoryCode","CategoryDescription","SerialFrom","SerialTo","CategoryStatus","ParentCategory","CGSAccountTitle","RevenueAccountTitle","InventoryAccountTitle","ClassGroupName","ProductionStage","VarietyNature"),List.of("Id","RevenueAccountId","InventoryAccountId","CGSAccountId","InventoryParentCategoriesId"));
            case "brands" -> new LayoutSpec("DefineBrand","grdfrm",876,List.of("BrandCode","BrandName","EntryUser","EntryDate","ModifyUser","ModifyDate"),List.of("Id"));
            case "uom-schedules" -> new LayoutSpec("InvDeffrmItemUomSchedule","grdfrm",115,List.of("Item","Schedule Unit","Equivalent","QtyEquivalent","BaseRateUom","BasePackUom","BaseSecondaryUom","Active"),List.of("Id"));
            case "opening-stock" -> new LayoutSpec("frmOpeningStockBlancing","DataGridHistory",92,List.of("DocNo","DocDate","Item","ItemUOM","Crop","JobLot","Warehouse","PackingType","StockCreditAccount","Qty","Weight","Rate","RateUOM","Amount","Remarks","IssueWeight","EntryDate","EntryUser","ModifyDate","ModifyUser","NoOfAttachments"),List.of("Id"));
            case "store-item-ledger" -> new LayoutSpec("InventoryEvaluationItemLedgerForStore","grd",288,List.of("BranchName","DocNo","TranDate","DocType","Description","WareHouse","RackName","ItemCategory","ItemType","ItemName","PackSize","ItemCondition","QtyIn","QtyOut","BalQty","WeightIn","WeightOut","BalWeight","AmountIn","AmountOut","BalAmount","ItemRate","AvgRate"),List.of("Id","DocumentTypeId","SupplierCustomerId","RackId"));
            default -> throw new IllegalArgumentException("Unknown inventory grid");
        };
    }
    public DesktopInventoryGridLayoutService(DesktopInventoryGridLayoutRepository repo,CurrentUserContext context,DesktopReportRights rights,@Value("${inventory.desktop-layout-directory:C:/SCS/Layout}") String root) {
        this.repo=repo;this.context=context;this.rights=rights;this.root=Path.of(root).toAbsolutePath().normalize();
    }
    private UserAccount user(LayoutSpec spec){var u=context.requireAccountingUser();rights.require(u,spec.screen,"View");return u;}
    private String name(UserAccount u,LayoutSpec spec){String username=u.getUserName();if(username==null||username.isBlank()||username.matches(".*[\\\\/:*?\"<>|].*")||username.endsWith(".")||username.endsWith(" "))throw new IllegalArgumentException("The desktop user name cannot be used for a layout file");String name=spec.form+"_"+spec.grid+"_"+username;if(name.length()>150)throw new IllegalArgumentException("The desktop layout name is too long");return name;}
    private Path path(String name){Path file=root.resolve(name).normalize();if(!root.equals(file.getParent()))throw new IllegalArgumentException("Invalid layout path");return file;}
    public List<InventoryGridColumn> load() throws IOException {return load("defined-items");}
    public List<InventoryGridColumn> load(String target) throws IOException {
        var spec=spec(target);var u=user(spec);String name=name(u,spec);Path file=path(name);if(!repo.exists(u,name)||!Files.isRegularFile(file))return List.of();
        try {
            return readColumns(parse(read(file)),spec);
        } catch (IllegalArgumentException ex) {
            return List.of();
        }
    }
    @Transactional(isolation=Isolation.SERIALIZABLE,rollbackFor=IOException.class)
    public List<InventoryGridColumn> save(List<InventoryGridColumn> columns) throws IOException {return save("defined-items",columns);}
    @Transactional(isolation=Isolation.SERIALIZABLE,rollbackFor=IOException.class)
    public List<InventoryGridColumn> save(String target,List<InventoryGridColumn> columns) throws IOException {
        var spec=spec(target);validate(columns,spec);var u=user(spec);String name=name(u,spec);Path file=path(name);byte[] before=Files.exists(file)?read(file):null;
        Document doc=before==null?empty():parse(before);Element rootTable=child(doc.getDocumentElement(),"RootTable");Element container=child(rootTable,"Columns");container.setAttribute("Collection","true");container.setAttribute("ElementName","Column");
        for(var column:columns){Element node=find(container,column.key);if(node==null){node=doc.createElement("Column"+nextIndex(container));node.setAttribute("ID",column.key);container.appendChild(node);put(node,"Key",column.key);put(node,"DataMember",column.key);put(node,"Caption",column.key);}put(node,"Width",String.valueOf(column.width));put(node,"Position",String.valueOf(column.position+(spec.screen==92?1:0)));put(node,"Visible",String.valueOf(column.visible));}
        // Keep desktop-only identifiers and selection out of the chooser.
        if(before==null){for(String key:spec.hidden){Element node=doc.createElement("Column"+nextIndex(container));node.setAttribute("ID",key);container.appendChild(node);put(node,"Key",key);if(!key.equals("Select"))put(node,"DataMember",key);put(node,"Visible","false");put(node,"Position",String.valueOf(columns.size()+spec.hidden.indexOf(key)+(spec.screen==92?1:0)));if(key.equals("Select")){put(node,"ActAsSelector","true");put(node,"UseHeaderSelector","true");put(node,"Width","30");}}}
        if(spec.screen==92){Element edit=find(container,"Edit");if(edit==null){edit=doc.createElement("Column"+nextIndex(container));edit.setAttribute("ID","Edit");container.appendChild(edit);put(edit,"Key","Edit");put(edit,"Caption","Edit");put(edit,"ButtonText","Edit");put(edit,"ButtonStyle","ButtonCell");put(edit,"ButtonDisplayMode","Always");put(edit,"Width","40");}put(edit,"Position","0");put(edit,"Visible","true");}
        repo.save(u,spec.form,name);replaceWithRollback(file,before,serialize(doc));return readColumns(doc,spec);
    }
    @Transactional(isolation=Isolation.SERIALIZABLE,rollbackFor=IOException.class)
    public void remove() throws IOException {remove("defined-items");}
    @Transactional(isolation=Isolation.SERIALIZABLE,rollbackFor=IOException.class)
    public void remove(String target) throws IOException {var spec=spec(target);var u=user(spec);String name=name(u,spec);Path file=path(name);byte[] before=Files.exists(file)?read(file):null;repo.remove(u,name);replaceWithRollback(file,before,null);}
    private static void validate(List<InventoryGridColumn> columns,LayoutSpec spec){if(columns==null||columns.size()!=spec.keys.size())throw new IllegalArgumentException("Every history column must have a layout setting");var keys=new HashSet<String>();var positions=new HashSet<Integer>();for(var c:columns){if(c==null||!spec.keys.contains(c.key)||!keys.add(c.key)||c.width<24||c.width>2000||c.position<0||c.position>=spec.keys.size()||!positions.add(c.position))throw new IllegalArgumentException("Invalid or duplicate history column setting");}if(columns.stream().noneMatch(c->c.visible))throw new IllegalArgumentException("Show at least one history column");}
    private static byte[] read(Path file)throws IOException{if(Files.size(file)>1024*1024)throw new IOException("Desktop grid layout exceeds 1 MB");return Files.readAllBytes(file);}
    private static Document parse(byte[] xml){try{var factory=DocumentBuilderFactory.newInstance();factory.setFeature("http://apache.org/xml/features/disallow-doctype-decl",true);factory.setFeature("http://xml.org/sax/features/external-general-entities",false);factory.setFeature("http://xml.org/sax/features/external-parameter-entities",false);factory.setAttribute(XMLConstants.ACCESS_EXTERNAL_DTD,"");factory.setAttribute(XMLConstants.ACCESS_EXTERNAL_SCHEMA,"");factory.setExpandEntityReferences(false);Document doc=factory.newDocumentBuilder().parse(new ByteArrayInputStream(xml));if(!doc.getDocumentElement().getTagName().equals("GridEXLayoutData"))throw new IllegalArgumentException("Not a Janus grid layout");return doc;}catch(Exception ex){throw new IllegalArgumentException("The desktop grid layout cannot be read",ex);}}
    private static Document empty(){return parse("<GridEXLayoutData><RootTable><Columns Collection=\"true\" ElementName=\"Column\"/></RootTable></GridEXLayoutData>".getBytes(java.nio.charset.StandardCharsets.UTF_8));}
    private static Element child(Element parent,String name){for(Node n=parent.getFirstChild();n!=null;n=n.getNextSibling())if(n instanceof Element e&&e.getTagName().equals(name))return e;Element e=parent.getOwnerDocument().createElement(name);parent.appendChild(e);return e;}
    private static Element find(Element parent,String key){for(Node n=parent.getFirstChild();n!=null;n=n.getNextSibling())if(n instanceof Element e&&key.equals(e.getAttribute("ID")))return e;return null;}
    private static int nextIndex(Element parent){int result=0;for(Node n=parent.getFirstChild();n!=null;n=n.getNextSibling())if(n instanceof Element e&&e.getTagName().matches("Column\\d+"))result=Math.max(result,Integer.parseInt(e.getTagName().substring(6))+1);return result;}
    private static void put(Element node,String key,String value){child(node,key).setTextContent(value);}
    private static List<InventoryGridColumn> readColumns(Document doc,LayoutSpec spec){var result=new ArrayList<InventoryGridColumn>();Element container=child(child(doc.getDocumentElement(),"RootTable"),"Columns");for(String key:spec.keys){Element node=find(container,key);if(node==null)continue;var c=new InventoryGridColumn();c.key=key;c.width=integer(node,"Width",100);c.position=integer(node,"Position",spec.keys.indexOf(key)+(spec.screen==92?1:0))-(spec.screen==92?1:0);c.visible=!"false".equalsIgnoreCase(child(node,"Visible").getTextContent());result.add(c);}return result;}
    private static int integer(Element node,String key,int fallback){try{return Integer.parseInt(child(node,key).getTextContent());}catch(NumberFormatException ex){return fallback;}}
    private static byte[] serialize(Document doc){try{var factory=TransformerFactory.newInstance();factory.setAttribute(XMLConstants.ACCESS_EXTERNAL_DTD,"");factory.setAttribute(XMLConstants.ACCESS_EXTERNAL_STYLESHEET,"");var transformer=factory.newTransformer();var output=new ByteArrayOutputStream();transformer.transform(new DOMSource(doc),new StreamResult(output));return output.toByteArray();}catch(Exception ex){throw new IllegalArgumentException("Unable to write grid layout",ex);}}
    private void replaceWithRollback(Path file,byte[] before,byte[] after)throws IOException{
        if(!TransactionSynchronizationManager.isSynchronizationActive())throw new IllegalStateException("Grid layout writes require a transaction");
        boolean registered=TransactionSynchronizationManager.getSynchronizations().stream().anyMatch(s->s instanceof LayoutRollback r&&r.file.equals(file));
        if(!registered)TransactionSynchronizationManager.registerSynchronization(new LayoutRollback(file,before));replace(file,after);
    }
    private class LayoutRollback implements TransactionSynchronization {
        private final Path file;private final byte[] before;
        LayoutRollback(Path file,byte[] before){this.file=file;this.before=before;}
        @Override public void afterCompletion(int status){if(status!=STATUS_COMMITTED)try{replace(file,before);}catch(IOException ex){throw new IllegalStateException("Failed to restore desktop grid layout after rollback",ex);}}
    }
    private void replace(Path file,byte[] content)throws IOException{if(content==null){Files.deleteIfExists(file);return;}Files.createDirectories(root);Path temporary=Files.createTempFile(root,"inventory-layout-",".tmp");try{Files.write(temporary,content);try{Files.move(temporary,file,StandardCopyOption.ATOMIC_MOVE,StandardCopyOption.REPLACE_EXISTING);}catch(AtomicMoveNotSupportedException ex){Files.move(temporary,file,StandardCopyOption.REPLACE_EXISTING);}}finally{Files.deleteIfExists(temporary);}}
}
