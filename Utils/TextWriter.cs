using System;
using System.IO;
using System.Text;

public class ConsoleInterceptor : IDisposable {
    private StreamWriter fileStream;
    private TextWriter doubleWriter;
    private TextWriter originalConsoleOut;

    public ConsoleInterceptor(string filePath) {
        originalConsoleOut = Console.Out;
        fileStream = new StreamWriter(filePath, true) {
            AutoFlush = true
        };
        doubleWriter = new DoubleWriter(originalConsoleOut, fileStream);
        Console.SetOut(doubleWriter);
    }

    public void Dispose() {
        Console.SetOut(originalConsoleOut); // Restore original console output
        doubleWriter.Dispose();
        fileStream.Dispose();
    }

    private class DoubleWriter : TextWriter {
        private TextWriter first;
        private TextWriter second;

        public DoubleWriter(TextWriter first, TextWriter second) {
            this.first = first;
            this.second = second;
        }

        public override Encoding Encoding => first.Encoding;

        public override void Write(char value) {
            first.Write(value);
            second.Write(value);
        }

        public override void WriteLine(string value) {
            first.WriteLine(value);
            second.WriteLine(value);
        }

        public override void WriteLine() {
            first.WriteLine();
            second.WriteLine();
        }
    }
}
