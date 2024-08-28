namespace BrainfuckInterpreter;

public class Interpreter(string Program)
{
    public int ProgramPointer { get; private set; } = 0;

    public int DataPointer { get; private set; }
    public byte[] Data { get; private set; } = new byte[512];
    public List<byte> Output { get; private set; } = new();
    public Queue<byte> InputBuffer { get; set; } = new();
    
    public void Step()
    {
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
                // no-op for now, just continue into loop
                break;
            case ']':
                while (Program[ProgramPointer] != '[')
                    ProgramPointer--;
                break;
            default:
                throw new NotImplementedException();
        }

        ProgramPointer++;
    }

    public void Input(byte input)
    {
        InputBuffer.Enqueue(input);
    }
}
