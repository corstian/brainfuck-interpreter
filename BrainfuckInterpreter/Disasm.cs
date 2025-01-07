using System.Numerics;

namespace BrainfuckInterpreter;

public class Disasm
{
    public int offset { get; set; }
    public string esil { get; set; }
    public int refptr { get; set; }
    public int fcn_addr { get; set; }
    public int fcn_last { get; set; }
    public int size { get; set; }
    public string opcode { get; set; }
    public string disasm { get; set; }
    public string bytes { get; set; }
    public string family { get; set; }
    public string type { get; set; }
    public bool reloc { get; set; }
    public long type_num { get; set; }
    public int type2_num { get; set; }
    public BigInteger ptr { get; set; }
    public BigInteger jump { get; set; }
    public int fail { get; set; }
    public BigInteger val { get; set; }
}