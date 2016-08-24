using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CUE.NET;
using System.Diagnostics;
using CUE.NET.Exceptions;
using CUE.NET.Devices.Keyboard;
using CUE.NET.Profiles;
using System.Drawing;
using CUE.NET.Devices.Keyboard.Enums;
using System.IO;

namespace corsair_CUE_dota
{
    class Program
    {
        static void Main()
        {
            CorsairKeyboardKeyId[] Health = {CorsairKeyboardKeyId.Escape, CorsairKeyboardKeyId.F1, CorsairKeyboardKeyId.F2, CorsairKeyboardKeyId.F3, CorsairKeyboardKeyId.F4, CorsairKeyboardKeyId.F5,
                                            CorsairKeyboardKeyId.F6, CorsairKeyboardKeyId.F7 };

            CorsairKeyboardKeyId[] Mana = {CorsairKeyboardKeyId.F8, CorsairKeyboardKeyId.F9, CorsairKeyboardKeyId.F10, CorsairKeyboardKeyId.F11, CorsairKeyboardKeyId.F12, CorsairKeyboardKeyId.PrintScreen,
                                            CorsairKeyboardKeyId.ScrollLock, CorsairKeyboardKeyId.PauseBreak };

            CueSDK.Initialize();
            Debug.WriteLine("Initialized with " + CueSDK.LoadedArchitecture + "-SDK");

            CorsairKeyboard keyboard = CueSDK.KeyboardSDK;
            if (keyboard == null)
                throw new WrapperException("No keyboard found");

            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "C:\\Python34\\python.exe";
            proc.StartInfo.Arguments = "-u \"C:\\Users\\Chris\\Documents\\Visual Studio 2015\\Projects\\dota_keyboard_prototype\\dota_keyboard_prototype\\dota_keyboard_prototype.py\"";


            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = false;
            proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;

            proc.Start();
            StreamReader q = proc.StandardOutput;
            while (!proc.HasExited)
            {
                String[] status = q.ReadLine().Split(null);
                Console.WriteLine(q.ReadLine());
                if (status[0] == "BG")
                    continue;

                int health_int = Int32.Parse(status[0]);
                int mana_int = Int32.Parse(status[1]);

                for (int i = 0; i < 8; i++)
                {
                    if (i < health_int)
                        keyboard[Health[i]].Led.Color = Color.Green;
                    else
                        keyboard[Health[i]].Led.Color = Color.Red;

                    if (i < mana_int)
                        keyboard[Mana[i]].Led.Color = Color.Blue;
                    else
                        keyboard[Mana[i]].Led.Color = Color.White;
                    keyboard.Update();
                }

            }
                
        }


    }
}
