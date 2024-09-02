using System.Text;

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

        Assert.Equal(0, interpreter.DataPointer);
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

    [Fact]
    public void BalanceLoops()
    {
        var interpreter = new Interpreter("[[,]]");

        interpreter.Run();
        
        Assert.Equal(5, interpreter.ProgramPointer);
    }
    
    [Fact]
    public void HelloWorld()
    {
        // Source: https://en.wikipedia.org/wiki/Brainfuck#Hello_World!
        var program = """
                      [ This program prints "Hello World!" and a newline to the screen; its
                        length is 106 active command characters. [It is not the shortest.]
                      
                        This loop is an "initial comment loop", a simple way of adding a comment
                        to a BF program such that you don't have to worry about any command
                        characters. Any ".", ",", "+", "-", "<" and ">" characters are simply
                        ignored, the "[" and "]" characters just have to be balanced. This
                        loop and the commands it contains are ignored because the current cell
                        defaults to a value of 0; the 0 value causes this loop to be skipped.
                      ]
                      ++++++++                Set Cell #0 to 8
                      [
                          >++++               Add 4 to Cell #1; this will always set Cell #1 to 4
                          [                   as the cell will be cleared by the loop
                              >++             Add 2 to Cell #2
                              >+++            Add 3 to Cell #3
                              >+++            Add 3 to Cell #4
                              >+              Add 1 to Cell #5
                              <<<<-           Decrement the loop counter in Cell #1
                          ]                   Loop until Cell #1 is zero; number of iterations is 4
                          >+                  Add 1 to Cell #2
                          >+                  Add 1 to Cell #3
                          >-                  Subtract 1 from Cell #4
                          >>+                 Add 1 to Cell #6
                          [<]                 Move back to the first zero cell you find; this will
                                              be Cell #1 which was cleared by the previous loop
                          <-                  Decrement the loop Counter in Cell #0
                      ]                       Loop until Cell #0 is zero; number of iterations is 8
                      
                      The result of this is:
                      Cell no :   0   1   2   3   4   5   6
                      Contents:   0   0  72 104  88  32   8
                      Pointer :   ^
                      
                      >>.                     Cell #2 has value 72 which is 'H'
                      >---.                   Subtract 3 from Cell #3 to get 101 which is 'e'
                      +++++++..+++.           Likewise for 'llo' from Cell #3
                      >>.                     Cell #5 is 32 for the space
                      <-.                     Subtract 1 from Cell #4 for 87 to give a 'W'
                      <.                      Cell #3 was set to 'o' from the end of 'Hello'
                      +++.------.--------.    Cell #3 for 'rl' and 'd'
                      >>+.                    Add 1 to Cell #5 gives us an exclamation point
                      >++.                    And finally a newline from Cell #6
                      """;
        
        var interpreter = new Interpreter(program);
    
        interpreter.Run();
        
        Assert.Equal("Hello World!\n", Encoding.ASCII.GetString(interpreter.Output.ToArray()));
    }
}
