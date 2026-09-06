import os
import glob
import re
import time

t0 = time.time()
SRC_DIR = r"d:\CShapEccorErp\src"
IL_DIR = r"d:\CShapEccorErp\il_by_namespace"

if not os.path.exists(SRC_DIR):
    os.makedirs(SRC_DIR, exist_ok=True)

il_files = glob.glob(os.path.join(IL_DIR, "*.txt"))
print(f"Total IL files to process: {len(il_files)}")

total_cs = 0

for il_path in il_files:
    fname = os.path.basename(il_path)
    if fname.startswith("_"):
        continue

    current_class_name = None
    methods = []
    fields_dict = {}

    with open(il_path, "r", encoding="utf-8", errors="ignore") as f:
        for line in f:
            line_str = line.strip()
            if not line_str:
                continue

            if "CLASS " in line_str and line_str.startswith("CLASS "):
                current_class_name = line_str[6:].strip()
            elif line_str.startswith("; ") and "::" in line_str:
                mname = line_str[2:].split("(")[0].strip()
                methods.append(mname)
            elif line_str.startswith("IL_"):
                parts = line_str.split(":", 1)
                if len(parts) == 2:
                    rest = parts[1].strip()
                    op_parts = rest.split(None, 1)
                    op = op_parts[0]
                    opnd = op_parts[1] if len(op_parts) > 1 else ""
                    if current_class_name and op in ("stfld", "ldfld", "stsfld", "ldsfld"):
                        if "::" in opnd:
                            f_class, f_name = opnd.split("::", 1)
                            if f_class == current_class_name:
                                clean_f = re.sub(r'[^a-zA-Z0-9_]', '_', f_name)
                                if clean_f and not clean_f[0].isalpha() and clean_f[0] != '_':
                                    clean_f = "_" + clean_f
                                is_static = op in ("stsfld", "ldsfld")
                                key = clean_f.lower()
                                if key not in fields_dict:
                                    fields_dict[key] = (clean_f, is_static)

    if not current_class_name or current_class_name.startswith("<") or current_class_name.startswith("TypeSpec"):
        continue

    parts = current_class_name.split(".")
    if len(parts) > 1:
        namespace = ".".join(parts[:-1])
        class_short = parts[-1]
    else:
        namespace = "Architecture.WinApp"
        class_short = current_class_name

    class_short = re.sub(r'[^a-zA-Z0-9_]', '_', class_short)
    if class_short[0].isdigit():
        class_short = "_" + class_short

    cs_lines = []
    cs_lines.append("using System;")
    cs_lines.append("using System.Collections.Generic;")
    cs_lines.append("using System.ComponentModel;")
    cs_lines.append("using System.Data;")
    cs_lines.append("using System.Drawing;")
    cs_lines.append("using System.Windows.Forms;")
    cs_lines.append("")
    cs_lines.append(f"namespace {namespace}")
    cs_lines.append("{")
    cs_lines.append(f"    public partial class {class_short} : Form")
    cs_lines.append("    {")

    # Deduplicated fields
    for key in sorted(fields_dict.keys()):
        fname_clean, is_static = fields_dict[key]
        if fname_clean == class_short:
            fname_clean = fname_clean + "_field"
        static_str = "static " if is_static else ""
        cs_lines.append(f"        public {static_str}object {fname_clean};")

    cs_lines.append("")
    cs_lines.append(f"        public {class_short}()")
    cs_lines.append("        {")
    cs_lines.append("            InitializeComponent();")
    cs_lines.append("        }")
    cs_lines.append("")
    cs_lines.append("        private void InitializeComponent()")
    cs_lines.append("        {")
    cs_lines.append("            this.SuspendLayout();")
    cs_lines.append(f'            this.Name = "{class_short}";')
    cs_lines.append(f'            this.Text = "{class_short}";')
    cs_lines.append("            this.ResumeLayout(false);")
    cs_lines.append("        }")
    cs_lines.append("")

    if class_short == "Program":
        cs_lines.append("        [STAThread]")
        cs_lines.append("        public static void Main()")
        cs_lines.append("        {")
        cs_lines.append("            Application.EnableVisualStyles();")
        cs_lines.append("            Application.SetCompatibleTextRenderingDefault(false);")
        cs_lines.append("            Application.Run(new LoginNew());")
        cs_lines.append("        }")
        cs_lines.append("")

    added_methods = set()
    for mfull in methods:
        mshort = mfull.split("::")[-1] if "::" in mfull else mfull
        mshort = re.sub(r'[^a-zA-Z0-9_]', '_', mshort)
        if mshort in ("_ctor", ".ctor", "_cctor", ".cctor", "Main", "InitializeComponent"):
            continue
        if mshort == class_short:
            mshort = mshort + "_Method"
        
        m_key = mshort.lower()
        # Avoid conflict with field names
        if m_key in fields_dict:
            mshort = mshort + "_Method"
            m_key = mshort.lower()

        if m_key not in added_methods:
            added_methods.add(m_key)
            cs_lines.append(f"        public void {mshort}()")
            cs_lines.append("        {")
            cs_lines.append("        }")
            cs_lines.append("")

    cs_lines.append("    }")
    cs_lines.append("}")

    folder_name = namespace.replace(".", "_")
    target_dir = os.path.join(SRC_DIR, folder_name)
    os.makedirs(target_dir, exist_ok=True)
    
    target_cs_path = os.path.join(target_dir, f"{class_short}.cs")
    with open(target_cs_path, "w", encoding="utf-8") as out_fp:
        out_fp.write("\n".join(cs_lines))

    total_cs += 1

t1 = time.time()
print(f"Done in {t1-t0:.2f} seconds! Updated {total_cs} C# files in {SRC_DIR}.")
