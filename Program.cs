BaseFactory baseFactory = new BaseFactory();
ToFileFactory toFileFactory = new ToFileFactory();

Client baseClient = new Client(baseFactory);
baseClient.Log("Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет Привет ");

Client toFileClient = new Client(toFileFactory);
toFileClient.Log("Хола");
Console.ReadKey();


public class Client
{
   private Logger logger { get; set; }
   private Drawer drawer { get; set; }

   public Client(LogMessageFactory factory)
   {
      logger = factory.CreateLogger();
      drawer = factory.CreateDrawer();
   }

   public void Log(string message)
   {
      string[] messageInDrawing = drawer.PutMessageInDrawing(message);
      logger.Log(messageInDrawing);
   }
}

public abstract class LogMessageFactory
{
   abstract public Logger CreateLogger();
   abstract public Drawer CreateDrawer();
}

public class BaseFactory : LogMessageFactory
{
   public override Logger CreateLogger()
   {
      return new ConsoleLogger();
   }
   public override Drawer CreateDrawer()
   {
      Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
      return new FormalDrawer(Console.BufferWidth - 3);
   }
}

public class ToFileFactory : LogMessageFactory
{
   public override Logger CreateLogger()
   {
      return new FileLogger();
   }
   public override Drawer CreateDrawer()
   {
      return new RectangleDrawer(80);
   }
}

public abstract class Logger
{
   abstract public void Log(string[] linesOfMessage);
}

public class ConsoleLogger : Logger
{
   public override void Log(string[] linesOfMessage)
   {
      Console.Clear();
      foreach(string line in linesOfMessage)
      {
         Console.WriteLine(line);
      }
   }
}

public class FileLogger : Logger
{
   public string FilePath { get; private set; }

   public FileLogger()
   {
      FilePath = Path.Combine(Environment.CurrentDirectory, "log.txt");
   }

   public override void Log(string[] linesOfMessage)
   {
      File.AppendAllLines(FilePath, linesOfMessage);
   }
}


public abstract class Drawer
{
   protected int LineWidth { get; private set; }
   public Drawer(int MessageWidth)
   {
      this.LineWidth = MessageWidth;
   }
   abstract public string[] PutMessageInDrawing(string message);
}

public class FormalDrawer : Drawer
{
   public FormalDrawer(int LineWidth) : base(LineWidth){}
   public override string[] PutMessageInDrawing(string message)
   {
      string currentTime = $"{DateTime.Now.ToString()}: ";
      int maxLength = LineWidth - currentTime.Length;
      List<string> LinesOfMessage = new List<string>();
      
      for(int i = 0; i < message.Length; i += maxLength)
      {
         string messageLine; 
         if(i + maxLength + 1 < message.Length - 1)
         {
            messageLine = currentTime + message.Substring(i, maxLength);
         }
         else
         {
            messageLine = currentTime + message.Substring(i);
         }
         LinesOfMessage.Add(messageLine);
      }
      return LinesOfMessage.ToArray();
   }
}

public class RectangleDrawer : Drawer
{
   public RectangleDrawer(int LineWidth) : base(LineWidth){}

   public override string[] PutMessageInDrawing(string message)
   {
      int maxLength = LineWidth - 4;
      List<string> LinesOfMessage = new List<string>();
      LinesOfMessage.Add(new string('_', LineWidth));
      
      for(int i = 0; i < message.Length; i += maxLength)
      {
         string messageLine; 
         if(i + maxLength + 1 < message.Length - 1)
         {
            messageLine = $"| {message.Substring(i, maxLength)} |";
         }
         else
         {
            string spaces = new string(' ', maxLength - message.Substring(i).Length);
            messageLine = $"| {message.Substring(i)}{spaces} |";
         }
         Console.WriteLine(messageLine);
         LinesOfMessage.Add(messageLine);
      }
      LinesOfMessage.Add(new string('-', LineWidth));
      return LinesOfMessage.ToArray();
   }
}