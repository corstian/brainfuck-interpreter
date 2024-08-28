namespace BrainfuckInterpreter;

public class InterpreterTests
{
    [Fact]
    public void InterpreterIsInitialized()
    {
        var interpreter = new Interpreter("");

        Assert.NotNull(interpreter);
        Assert.Equal(0, interpreter.DataPointer);
    }

    [Fact]
    public void IncreasesDataPointer()
    {
        var interpreter = new Interpreter(">");

        interpreter.Step();
        
        Assert.Equal(1, interpreter.DataPointer);
    }

    [Fact]
    public void DecreasesDataPointer()
    {
        var interpreter = new Interpreter("<");

        interpreter.Step();

        Assert.Equal(-1, interpreter.DataPointer);
    }

    [Fact]
    public void OutputsByte()
    {
        var interpreter = new Interpreter(".");

        interpreter.Step();
        
        Assert.Equal([0x00] , interpreter.Output);
    }

    [Fact]
    public void IncremetsByte()
    {
        var interpreter = new Interpreter("+.");
        
        interpreter.Step();
        interpreter.Step();
        
        Assert.Equal([0x01], interpreter.Output);
    }

    [Fact]
    public void DecrementsByte()
    {
        var interpreter = new Interpreter("-.");

        interpreter.Step();
        interpreter.Step();
        
        Assert.Equal([0xff], interpreter.Output);
    }

    [Fact]
    public void AcceptInput()
    {
        var interpreter = new Interpreter(",.");

        interpreter.Input(0xaa);
        interpreter.Step();
        interpreter.Step();

        Assert.Equal([0xaa], interpreter.Output);
    }

    [Fact]
    public void TrySkipLoop()
    {
        var interpreter = new Interpreter("[");

        interpreter.Step();
        
        Assert.Equal(1, interpreter.ProgramPointer);
    }

    [Fact]
    public void TryStartLoop()
    {
        var interpreter = new Interpreter("+[");

        interpreter.Run();
        
        Assert.Equal(2, interpreter.ProgramPointer);
    }

    [Fact]
    public void TryEndLoop()
    {
        var interpreter = new Interpreter("]");

        interpreter.Run();
        Assert.Equal(1, interpreter.ProgramPointer);
    }

    [Fact]
    public void SkipsLoopIfDataAtPointerIsZero()
    {
        var interpreter = new Interpreter("[]");

        interpreter.Step();
        
        Assert.Equal(2, interpreter.ProgramPointer);
    }

    [Fact]
    public void GoesIntoLoopIfDataAtPointerIsNonZero()
    {
        var interpreter = new Interpreter("+[]");
        
        interpreter.Step();
        interpreter.Step();
        
        Assert.Equal(2, interpreter.ProgramPointer);
    }

    [Fact]
    public void EndsLoop()
    {
        var interpreter = new Interpreter("+[-]");

        interpreter.Run();
        
        Assert.Equal(4, interpreter.ProgramPointer);
    }
}
