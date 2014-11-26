using System;
using System.Linq;
using System.Net.Sockets;

using System.Threading;
using System.IO;
using System.Security.Cryptography;


using Dirac;
using Dirac.Logging;
using Dirac.GameServer.Core;

using Dirac.GameServer.Network.Message;



namespace Dirac.GameServer.Network
{
    public sealed class GameClient
    {

        /*public BattleNetClient BnetClient { get; set; }
        public Account account;
        public GameAccount gameaccount;*/

        public GameServer gameserver;

        public StateObject clientobjectstate;

        public Socket Socket;

        private readonly GameBitBuffer _incomingBuffer = new GameBitBuffer(ushort.MaxValue);
        private readonly GameBitBuffer _outgoingBuffer = new GameBitBuffer(ushort.MaxValue);

        public Boolean IsLoggingOut;
        public Boolean IsNetBufferDirty { get; set; }
        public Player Player { get; set; }

        public bool TickingEnabled { get; set; }

        public int TickCounter = 0;

        private object _bufferLock = new object(); // we should be locking on this private object, locking on gameclient (this) may cause deadlocks. detailed information: http://msdn.microsoft.com/fr-fr/magazine/cc188793%28en-us%29.aspx /raist.

        public GameClient()
        {
            this.TickingEnabled = true;
            /*this.Connection = connection;*/
            _outgoingBuffer.WriteInt(32, 0);
        }

        public void Parse(Byte[] e)
        {

            _incomingBuffer.AppendData(e.ToArray());

            while (_incomingBuffer.IsPacketAvailable())
            {
                int end = _incomingBuffer.Position;
                end += _incomingBuffer.ReadInt(32) * 8;

                while ((end - _incomingBuffer.Position) >= 9)
                {
                    var message = _incomingBuffer.ParseMessage();
                    if (message == null) 
                        continue;
                    try
                    {
                        //LogPacket.LogIncomingPacket(message);
                        //Program.serverForm.LogPacket("[IN] " + message.GetType().Name, Program.serverForm.customColorStyle1);

                        if (message.Consumer != Consumers.None)
                        {
                            /*if (message.Consumer == Consumers.ClientManager)
                                ClientManager.Consume(this, message); //initial message
                            else*/
                                Game.Route(this, message);
                        }
                        else
                        {
                            if (message is ISelfHandler) 
                                (message as ISelfHandler).Handle(this);
                            else
                                Logging.LogManager.DefaultLogger.Warn("{0} - ID:{1} has no consumer or self-handler.", message.GetType(), message.Id);
                        }
                        

                    }
                    catch (NotImplementedException)
                    {
                        Logging.LogManager.DefaultLogger.Error("Unhandled game message: 0x{0:X4} {1}", message.Id, message.GetType().Name);
                    }
                    catch (Exception ex)
                    {
                        Logging.LogManager.DefaultLogger.Error(ex.Message);
                    }
                }

                _incomingBuffer.Position = end;
            }
            _incomingBuffer.ConsumeData();
        }
        
        public void SendMessage(GameMessage message, bool flushImmediatly = false)
        {
            lock (this._bufferLock)
            {
                //Logging.Logger.PacketLog(message.GetType().Name);
                //Program.serverForm.LogPacket("[OUT] " + message.GetType().Name, Program.serverForm.customColorStyle2);
                //LogPacket.LogOutgoingPacket(message);
                _outgoingBuffer.EncodeMessage(message); // change ConsoleTarget's level to Level.Dump in program.cs if u want to see messages on console.
                this.IsNetBufferDirty = true;
                if (flushImmediatly)
                {
                    //Interlocked.Add(ref this.Player.TickCounter, 1); 
                    this.SendTick();
                }
            }
        }

        public void SendTick()
        {
            lock (this._bufferLock)
            {
                if (_outgoingBuffer.Length <= 32) 
                    return;

                if (this.TickingEnabled)
                {
                    Interlocked.Add(ref this.TickCounter, 1);
                    
                    this.SendMessage(new GameTickMessage(this.TickCounter)); // send the tick.
                }
                this.FlushOutgoingBuffer();
                this.IsNetBufferDirty = false;
            }
        }

        private static int count;
        private void FlushOutgoingBuffer()
        {
            lock (this._bufferLock)
            {
                if (_outgoingBuffer.Length <= 32) 
                    return;

                Byte[] data = _outgoingBuffer.GetPacketAndReset();
                this.Send(data);
            }
        }

        public int Send(Byte[] data)
        {
            if (data == null)
                throw new Exception("Data parameter is NULL on GameClient");
            return this.Socket.Send(data, 0, data.Length, SocketFlags.None);
        }

        public void Disconnect()
        {
            try
            {
                this.Socket.Shutdown(SocketShutdown.Both);
                this.Socket.Disconnect(true);
            }
            catch (Exception ex)
            {
                Logging.LogManager.DefaultLogger.Error(ex.Message);
            }
        }

    }


    class TripleDESManagedExample
    {
        public static void Main2()
        {
            try
            {
                string original = "Here is some data to encrypt!";

                // Create a new instance of the TripleDESCryptoServiceProvider
                // class.  This generates a new key and initialization 
                // vector (IV).
                using (TripleDESCryptoServiceProvider myTripleDES = new TripleDESCryptoServiceProvider())
                {
                    // Encrypt the string to an array of bytes.
                    byte[] encrypted = EncryptStringToBytes(original, myTripleDES.Key, myTripleDES.IV);

                    // Decrypt the bytes to a string.
                    string roundtrip = DecryptStringFromBytes(encrypted, myTripleDES.Key, myTripleDES.IV);

                    //Display the original data and the decrypted data.
                    Console.WriteLine("Original:   {0}", original);
                    Console.WriteLine("Round Trip: {0}", roundtrip);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }
        static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an TripleDESCryptoServiceProvider object
            // with the specified key and IV.
            using (TripleDESCryptoServiceProvider tdsAlg = new TripleDESCryptoServiceProvider())
            {
                tdsAlg.Key = Key;
                tdsAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = tdsAlg.CreateEncryptor(tdsAlg.Key, tdsAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an TripleDESCryptoServiceProvider object
            // with the specified key and IV.
            using (TripleDESCryptoServiceProvider tdsAlg = new TripleDESCryptoServiceProvider())
            {
                tdsAlg.Key = Key;
                tdsAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = tdsAlg.CreateDecryptor(tdsAlg.Key, tdsAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }
    }

}
