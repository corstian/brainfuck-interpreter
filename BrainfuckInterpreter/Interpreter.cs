namespace BrainfuckInterpreter;

public class Interpreter(string Program)
{
    public int OperationCount { get; private set; } = 0;
    
    public int ProgramPointer { get; private set; } = 0;

    public int DataPointer { get; private set; }
    public byte[] Data { get; private set; } = new byte[512];
    public List<byte> Output { get; private set; } = new();
    public Queue<byte> InputBuffer { get; set; } = new();
    
    public void Step()
    {
        if (ProgramPointer >= Program.Length) return;
        
        switch (Program[ProgramPointer])
        {
            case '>':
                DataPointer++;
                break;
            case '<':
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
                Data[DataPointer] = InputBuffer.Dequeue();
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
    }

    public void Input(byte input)
    {
        InputBuffer.Enqueue(input);
    }

    public void Run()
    {
        while (ProgramPointer < Program.Length)
        {
            Step();
        }
    }
}
