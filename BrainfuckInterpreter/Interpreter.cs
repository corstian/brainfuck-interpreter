namespace BrainfuckInterpreter;

public class Interpreter(string Program)
{
    public int OperationCount { get; private set; } = 0;
    
    public int ProgramPointer { get; private set; } = 0;

    public int DataPointer { get; private set; }
    public byte[] Data { get; private set; } = new byte[512];
    public List<byte> Output { get; private set; } = new();
    public Queue<byte> InputBuffer { get; set; } = new();
    
    public bool Step()
    {
        if (ProgramPointer >= Program.Length) return false;
        
        switch (Program[ProgramPointer])
        {
            case '>':
                if (DataPointer < Data.Length)
                    DataPointer++;
                break;
            case '<':
                if (DataPointer > 0)
                    DataPointer--;
                break;
            case '+':
                Data[DataPointer]++;
                break;
            case '-':
                Data[DataPointer]--;
                break;
            case '.':
                Output.Add(Data[DataPointer]);
                break;
            case ',':
                if (InputBuffer.TryDequeue(out var b))
                    Data[DataPointer] = b;
                break;
            case '[':
                if (Data[DataPointer] == 0)
                {
                    // Seek forward
                    var balance = 1;

                    char c;
                    
                    while (balance > 0 && ProgramPointer + 1 < Program.Length)
                    {
                        ProgramPointer++;
                        
                        c = Program[ProgramPointer];

                        if (c == '[') balance++;
                        if (c == ']') balance--;
                    }
                }

                break;
            case ']':
                if (Data[DataPointer] != 0)
                {
                    var balance = 1;
                    char c;
                    while (balance > 0
                           && ProgramPointer > 0)
                    {
                        ProgramPointer--;

                        c = Program[ProgramPointer];

                        if (c == ']') balance++;
                        if (c == '[') balance--;
                    }
                }

                break;
            default:
                // throw new NotImplementedException();
                break;
        }

        ProgramPointer++;
        OperationCount++;

        return true;
    }

    public void Input(byte input)
    {
        InputBuffer.Enqueue(input);
    }

    public void Run()
    {
        while (Step()) { }
    }
}
