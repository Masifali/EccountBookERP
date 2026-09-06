import os
import glob
import re

SRC_DIR = r"d:\CShapEccorErp\src"
IL_DIR = r"d:\CShapEccorErp\il_by_namespace"

def clean_type_name(full_type):
    if not full_type:
        return "object"
    full_type = full_type.strip()
    if full_type.startswith("System.Windows.Forms."):
        return full_type.split(".")[-1]
    if full_type.startswith("System.Drawing."):
        return full_type.split(".")[-1]
    if full_type.startswith("System.Data."):
        return full_type.split(".")[-1]
    if full_type == "System.String":
        return "string"
    if full_type in ("System.Int32", "System.Int64"):
        return "int"
    if full_type == "System.Boolean":
        return "bool"
    if full_type == "System.Object":
        return "object"
    if full_type == "System.Void":
        return "void"
    return full_type.split(".")[-1]

def parse_il_file(filepath):
    with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
        lines = f.readlines()

    classes = {}
    current_class = None
    current_method = None

    class_re = re.compile(r"^====================================================================================================\s*CLASS\s+(.+)$")
    method_re = re.compile(r"^;\s*([^\s(]+)(?:\s*\((.*)\))?")
    il_re = re.compile(r"^\s*IL_[0-9a-fA-F]+:\s*([^\s]+)(?:\s+(.*))?$")

    for line in lines:
        line_str = line.strip()
        if not line_str:
            continue
        
        cm = class_re.match(line_str)
        if cm:
            cname = cm.group(1).strip()
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
            
            # Field tracking
            if current_class and op in ("stfld", "ldfld", "stsfld", "ldsfld"):
                if "::" in opnd:
                    f_class, f_name = opnd.split("::", 1)
                    if f_class == current_class["name"]:
                        is_static = op in ("stsfld", "ldsfld")
                        current_class["fields"].add((f_name, is_static))

    return classes

def transpile_method(m, class_name):
    full_mname = m["full_name"]
    short_mname = full_mname.split("::")[-1] if "::" in full_mname else full_mname
    
    is_ctor = (short_mname == ".ctor")
    is_cctor = (short_mname == ".cctor")
    
    class_short_name = class_name.split(".")[-1]
    
    body_lines = []
    
    # Process IL instructions to reconstruct C# code
    insts = m["instructions"]
    for op, opnd in insts:
        # Clean string literal
        if op == "ldstr":
            clean_str = opnd
            if clean_str.startswith("ldstr "):
                clean_str = clean_str[6:]
            body_lines.append(f"            // {clean_str}")
        elif op == "call" or op == "callvirt":
            if "::" in opnd:
                target_cls, target_m = opnd.split("::", 1)
                target_cls_short = target_cls.split(".")[-1]
                if target_m.startswith(".ctor"):
                    body_lines.append(f"            // new {target_cls_short}();")
                else:
                    body_lines.append(f"            // {target_cls_short}.{target_m}();")
        elif op == "newobj":
            if "::" in opnd:
                target_cls = opnd.split("::")[0].split(".")[-1]
                body_lines.append(f"            // new {target_cls}();")
        elif op == "stfld" or op == "stsfld":
            if "::" in opnd:
                fname = opnd.split("::")[-1]
                body_lines.append(f"            this.{fname} = null; // assigned")

    code = []
    if is_ctor:
        code.append(f"        public {class_short_name}()")
        code.append("        {")
        code.append(f"            InitializeComponent();")
        code.append("        }")
    elif is_cctor:
        code.append(f"        static {class_short_name}()")
        code.append("        {")
        code.append("        }")
    else:
        ret_type = "void"
        static_kw = "static " if "Program" in class_name and short_mname == "Main" else ""
        if short_mname == "Main":
            code.append("        [STAThread]")
            code.append(f"        public {static_kw}void Main()")
            code.append("        {")
            code.append("            Application.EnableVisualStyles();")
            code.append("            Application.SetCompatibleTextRenderingDefault(false);")
            code.append("            Application.Run(new LoginNew());")
            code.append("        }")
        else:
            code.append(f"        public {static_kw}{ret_type} {short_mname}()")
            code.append("        {")
            if body_lines:
                code.extend(body_lines[:15]) # Sample top lines
            code.append("        }")

    return "\n".join(code)

def generate_cs_file(cname, cls_data):
    parts = cname.split(".")
    if len(parts) > 1:
        namespace = ".".join(parts[:-1])
        class_short = parts[-1]
    else:
        namespace = "Architecture.WinApp"
        class_short = cname

    # Determine base class
    is_form = False
    is_user_control = False
    for m in cls_data["methods"]:
        for op, opnd in m["instructions"]:
            if "System.Windows.Forms.Form::.ctor" in opnd:
                is_form = True
            elif "System.Windows.Forms.UserControl::.ctor" in opnd:
                is_user_control = True

    base_class = " : Form" if is_form else (" : UserControl" if is_user_control else "")

    lines = []
    lines.append("using System;")
    lines.append("using System.Collections.Generic;")
    lines.append("using System.ComponentModel;")
    lines.append("using System.Data;")
    lines.append("using System.Drawing;")
    lines.append("using System.Windows.Forms;")
    lines.append("")
    lines.append(f"namespace {namespace}")
    lines.append("{")
    lines.append(f"    public partial class {class_short}{base_class}")
    lines.append("    {")

    # Fields
    for fname, is_static in sorted(cls_data["fields"]):
        static_str = "static " if is_static else ""
        lines.append(f"        public {static_str}object {fname};")

    lines.append("")
    
    # Check if ctor exists
    has_ctor = any(m["full_name"].endswith("::.ctor") for m in cls_data["methods"])
    if not has_ctor and (is_form or is_user_control):
        lines.append(f"        public {class_short}()")
        lines.append("        {")
        lines.append("            InitializeComponent();")
        lines.append("        }")
        lines.append("")

    if is_form or is_user_control:
        lines.append("        private void InitializeComponent()")
        lines.append("        {")
        lines.append("            this.SuspendLayout();")
        lines.append("            // ")
        lines.append(f"            // {class_short}")
        lines.append("            // ")
        lines.append(f'            this.Name = "{class_short}";')
        lines.append(f'            this.Text = "{class_short}";')
        lines.append("            this.ResumeLayout(false);")
        lines.append("        }")
        lines.append("")

    for m in cls_data["methods"]:
        m_code = transpile_method(m, cname)
        lines.append(m_code)
        lines.append("")

    lines.append("    }")
    lines.append("}")

    return "\n".join(lines), namespace, class_short

print("Transpiler script ready.")
