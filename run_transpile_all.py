import os
import glob
import re

SRC_DIR = r"d:\CShapEccorErp\src"
IL_DIR = r"d:\CShapEccorErp\il_by_namespace"

if not os.path.exists(SRC_DIR):
    os.makedirs(SRC_DIR, exist_ok=True)

il_files = glob.glob(os.path.join(IL_DIR, "*.txt"))
print(f"Processing {len(il_files)} IL files...")

class_re = re.compile(r"^====================================================================================================\s*CLASS\s+(.+)$")
method_re = re.compile(r"^;\s*([^\s(]+)(?:\s*\((.*)\))?")
il_re = re.compile(r"^\s*IL_[0-9a-fA-F]+:\s*([^\s]+)(?:\s+(.*))?$")

total_cs_created = 0

for il_path in il_files:
    fname = os.path.basename(il_path)
    if fname.startswith("_"):
        continue # Skip manifest / compiler generated files for main source

    with open(il_path, "r", encoding="utf-8", errors="ignore") as f:
        lines = f.readlines()

    classes = {}
    current_class = None
    current_method = None

    for line in lines:
        line_str = line.strip()
        if not line_str:
            continue
        
        cm = class_re.match(line_str)
        if cm:
            cname = cm.group(1).strip()
            # Clean up generic markers or module markers
            if cname.startswith("<") or cname.startswith("TypeSpec"):
                current_class = None
                continue
            current_class = {
                "name": cname,
                "fields": set(),
                "methods": []
            }
            classes[cname] = current_class
            current_method = None
            continue

        mm = method_re.match(line_str)
        if mm:
            mname = mm.group(1).strip()
            mhdr = mm.group(2) or ""
            current_method = {
                "full_name": mname,
                "header": mhdr,
                "instructions": []
            }
            if current_class:
                current_class["methods"].append(current_method)
            continue

        im = il_re.match(line_str)
        if im and current_method:
            op = im.group(1).strip()
            opnd = im.group(2).strip() if im.group(2) else ""
            current_method["instructions"].append((op, opnd))
            
            if current_class and op in ("stfld", "ldfld", "stsfld", "ldsfld"):
                if "::" in opnd:
                    f_class, f_name = opnd.split("::", 1)
                    if f_class == current_class["name"]:
                        is_static = op in ("stsfld", "ldsfld")
                        current_class["fields"].add((f_name, is_static))

    # Generate CS file for each valid class
    for cname, cls_data in classes.items():
        parts = cname.split(".")
        if len(parts) > 1:
            namespace = ".".join(parts[:-1])
            class_short = parts[-1]
        else:
            namespace = "Architecture.WinApp"
            class_short = cname

        # Sanitize class name
        class_short = re.sub(r'[^a-zA-Z0-9_]', '_', class_short)
        if class_short[0].isdigit():
            class_short = "_" + class_short

        # Base class check
        is_form = False
        is_user_control = False
        for m in cls_data["methods"]:
            for op, opnd in m["instructions"]:
                if "System.Windows.Forms.Form::.ctor" in opnd:
                    is_form = True
                elif "System.Windows.Forms.UserControl::.ctor" in opnd:
                    is_user_control = True

        base_class = " : Form" if is_form else (" : UserControl" if is_user_control else "")

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
        cs_lines.append(f"    public partial class {class_short}{base_class}")
        cs_lines.append("    {")

        # Fields
        for fname, is_static in sorted(cls_data["fields"]):
            clean_fname = re.sub(r'[^a-zA-Z0-9_]', '_', fname)
            static_str = "static " if is_static else ""
            cs_lines.append(f"        public {static_str}object {clean_fname};")

        cs_lines.append("")
        
        has_ctor = any(m["full_name"].endswith("::.ctor") for m in cls_data["methods"])
        if not has_ctor and (is_form or is_user_control):
            cs_lines.append(f"        public {class_short}()")
            cs_lines.append("        {")
            cs_lines.append("            InitializeComponent();")
            cs_lines.append("        }")
            cs_lines.append("")

        if is_form or is_user_control:
            cs_lines.append("        private void InitializeComponent()")
            cs_lines.append("        {")
            cs_lines.append("            this.SuspendLayout();")
            cs_lines.append("            // ")
            cs_lines.append(f"            // {class_short}")
            cs_lines.append("            // ")
            cs_lines.append(f'            this.Name = "{class_short}";')
            cs_lines.append(f'            this.Text = "{class_short}";')
            cs_lines.append("            this.ResumeLayout(false);")
            cs_lines.append("        }")
            cs_lines.append("")

        for m in cls_data["methods"]:
            full_mname = m["full_name"]
            short_mname = full_mname.split("::")[-1] if "::" in full_mname else full_mname
            short_mname = re.sub(r'[^a-zA-Z0-9_]', '_', short_mname)
            
            is_ctor = (short_mname == "_ctor" or short_mname == ".ctor")
            is_cctor = (short_mname == "_cctor" or short_mname == ".cctor")

            if is_ctor:
                cs_lines.append(f"        public {class_short}()")
                cs_lines.append("        {")
                cs_lines.append("            InitializeComponent();")
                cs_lines.append("        }")
            elif is_cctor:
                cs_lines.append(f"        static {class_short}()")
                cs_lines.append("        {")
                cs_lines.append("        }")
            elif class_short == "Program" and short_mname == "Main":
                cs_lines.append("        [STAThread]")
                cs_lines.append("        public static void Main()")
                cs_lines.append("        {")
                cs_lines.append("            Application.EnableVisualStyles();")
                cs_lines.append("            Application.SetCompatibleTextRenderingDefault(false);")
                cs_lines.append("            Application.Run(new LoginNew());")
                cs_lines.append("        }")
            else:
                cs_lines.append(f"        public void {short_mname}()")
                cs_lines.append("        {")
                cs_lines.append("        }")

            cs_lines.append("")

        cs_lines.append("    }")
        cs_lines.append("}")

        # Save under appropriate folder
        folder_name = namespace.replace(".", "_")
        target_dir = os.path.join(SRC_DIR, folder_name)
        os.makedirs(target_dir, exist_ok=True)
        
        target_cs_path = os.path.join(target_dir, f"{class_short}.cs")
        with open(target_cs_path, "w", encoding="utf-8") as out_fp:
            out_fp.write("\n".join(cs_lines))

        total_cs_created += 1

print(f"Successfully created {total_cs_created} C# source files in {SRC_DIR}!")
