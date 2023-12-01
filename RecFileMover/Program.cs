using System.Diagnostics;
using System.Globalization;

namespace RecFileMover
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Clear();
                ConsoleColor c = Console.ForegroundColor;
                if (args.Length < 2)
                {
                    Console.WriteLine("引数に移動元ディレクトリと移動先ディレクトリを指定して起動してください。");
                    string[] newArgs = new string[2];
                    Console.WriteLine("移動元ディレクトリを入力してください。");
                    newArgs[0] = Console.ReadLine().Replace("\"", "");//"D:\Ichihai1415\video\_recording""D:\Ichihai1415\video\_recording\auto_dir"
                    if (newArgs[0] == "pass")//デバッグ用
                    {
                        newArgs[0] = "D:\\Ichihai1415\\video\\_recording";
                        newArgs[1] = "D:\\Ichihai1415\\video\\_recording\\auto_dir";
                        Console.WriteLine("「終了しました。」の表示の後に新しく起動します。");
                        Process.Start("RecFileMover.exe", newArgs);
                        goto end;
                    }
                    Console.WriteLine("移動先ディレクトリを入力してください。");
                    newArgs[1] = Console.ReadLine().Replace("\"", "");
                    Console.WriteLine("「終了しました。」の表示の後に新しく起動します。");
                    Process.Start("RecFileMover.exe", newArgs);
                    goto end;
                }
                args[0] = args[0].Replace("\"", "");
                args[1] = args[1].Replace("\"", "");
                Console.WriteLine("From:" + args[0]);
                Console.WriteLine(" To :" + args[1]);
                Console.WriteLine("移動してもよろしいですか?(y/n)");
                string? ans = args.Length == 3 ? "y" : Console.ReadLine();
                if (ans != "y")
                    goto end;
                foreach (string file in Directory.EnumerateFiles(args[0], "*.mkv"))
                {
                    string fileN = file.Replace($"{args[0]}\\", "").Replace($"{args[0]}", "");
                    bool ok = DateTime.TryParseExact(fileN, "yyyy-MM-dd HH-mm-ss'.mkv'", null, DateTimeStyles.None, out DateTime t);
                    if (!ok)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine($"{file} -> (対象外)");
                        Console.ForegroundColor = c;
                        continue;
                    }
                    string dir = $"{args[1]}\\{t:yyyy-MM-dd}\\".Replace("\\\\", "\\");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{file} -> {dir}{fileN}");
                    Console.ForegroundColor = c;
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    try
                    {
                        File.Move(file, dir + fileN);
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(e);
                        Console.ForegroundColor = c;
                    }
                }
                Console.ForegroundColor = c;
            end:
                Console.WriteLine("終了しました。");

            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
            }
        }
    }
}
