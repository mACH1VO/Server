using System;
using System.Collections.Generic;
using Dirac;
using Dirac.Logging;
using System.Linq;

using Dirac.GameServer.Network;
using Dirac.GameServer.Network.Message;

namespace Dirac.GameServer.Core
{
    public class GameAttributes
    {
        private Queue<Int32> _changedAttributesQueue = new Queue<Int32>();
        private Dictionary<Int32, AttributeValue> _currentAttributeMap = new Dictionary<Int32, AttributeValue>();
        private DynamicObject owner;

        public GameAttributes(DynamicObject parent)
        {
            owner = parent;
        }

        public bool doiHavethisAttribute(Attribute attribute, int? Field0)
        {
            return _currentAttributeMap.Keys.Contains(attribute.Id);
        }


        public void SendAllAttributes(GameClient client)
        {
            //return;
            var list = _getAllMessageList();
            foreach (var msg in list)
            {
                if (client != null)
                    client.SendMessage(msg);
            }
        }

        public void SendAllAttributes(IEnumerable<GameClient> clients)
        {
            //return;
            var list = _getAllMessageList();
            foreach (var msg in list)
            {
                foreach (var client in clients)
                    client.SendMessage(msg);
            }
        }

        /// <summary>
        /// Envia el mensaje cambiado solo a si mismo, solo se usa en inventario
        /// </summary>
        public void SendChangedMessage(GameClient client)
        {
            //return;
            var list = GetChangedMessageList();
            foreach (var msg in list)
                client.SendMessage(msg);
            _changedAttributesQueue.Clear(); //solo para changed 
        }

        
        public void SendChangedMessage(IEnumerable<GameClient> clients)
        {
            //return;
            if (_changedAttributesQueue.Count == 0)
                return;

            var list = GetChangedMessageList();
            foreach (var msg in list)
            {
                foreach (var client in clients)
                    client.SendMessage(msg);
            }
            _changedAttributesQueue.Clear();//solo para changed 
        }

        /// <summary>
        /// Broadcasts attribs to players that the parent actor has been revealed to.
        /// </summary>
        public void BroadcastAllAttributesIfRevealed()
        {
            List<GameClient> gameclients = new List<GameClient>();
            if (this.owner is Actor)
            {
                foreach (var p in (owner as Actor).World.Players.Values)
                {
                    if (p.RevealedObjects.ContainsKey(owner.DynamicID))
                        gameclients.Add(p.GameClient);

                }
                SendAllAttributes(gameclients);
            }
            /*SendAllAttributes(_parent.World.Players.Values
                .Where(@player => @player.RevealedObjects.ContainsKey(_parent.DynamicID))
                .Select(@player => @player.InGameClient));*/
        }

        public void FillDataFrom(GameAttributes gameattributes)
        {
            this._currentAttributeMap.Clear();
            this._changedAttributesQueue.Clear();
            foreach (var keyval in gameattributes.GetCurrentAttributeMap())
            {
                this._currentAttributeMap.Add(keyval.Key, keyval.Value);
                this._changedAttributesQueue.Enqueue(keyval.Key);
            }
        }

        public Dictionary<Int32, AttributeValue> GetCurrentAttributeMap()
        {
            return this._currentAttributeMap;
        }
        /// <summary>
        /// Broadcasts attribs to players that the parent actor has been revealed to.
        /// </summary>
        public void BroadcastAllAttributestoPlayer(Player player)
        {
            //return;
            if (player.GameClient != null)
                SendAllAttributes(player.GameClient);
            /*SendAllAttributes(_parent.World.Players.Values
                .Where(@player => @player.RevealedObjects.ContainsKey(_parent.DynamicID))
                .Select(@player => @player.InGameClient));*/
        }

        /// <summary>
        /// Broadcasts changed attribs to players that the parent actor has been revealed to.
        /// </summary>
        public void BroadcastChangedIfRevealed()
        {
            //return;
            List<GameClient> gameclients = new List<GameClient>();
            if (this.owner is Actor)
            {
                foreach (var p in (owner as Actor).World.Players.Values)
                {
                    if (p.RevealedObjects.ContainsKey(owner.DynamicID))
                        gameclients.Add(p.GameClient);

                }
                SendChangedMessage(gameclients);
            }

            /*SendChangedMessage(_parent.World.Players.Values
                .Where(@player => @player.RevealedObjects.ContainsKey(_parent.DynamicID))
                .Select(@player => @player.InGameClient));*/
        }

        public void BroadcastChangestoOwner()
        {
            //return;
            List<GameClient> gameclients = new List<GameClient>();
            if (this.owner is Player)
            {
                gameclients.Add((this.owner as Player).GameClient);
                SendChangedMessage(gameclients);
            }

            /*SendChangedMessage(_parent.World.Players.Values
                .Where(@player => @player.RevealedObjects.ContainsKey(_parent.DynamicID))
                .Select(@player => @player.InGameClient));*/
        }

        private List<GameMessage> _getAllMessageList()
        {
            Queue<Int32> _changedAttributesQueueForthisFunction = new Queue<Int32>();

            foreach (var atrib in _currentAttributeMap.Keys)
            {
                _changedAttributesQueueForthisFunction.Enqueue(atrib);
            }

            var messageList = new List<GameMessage>();
            if (_changedAttributesQueueForthisFunction.Count == 0)
                return messageList;
            /*if (_changedAttributesQueueForthisFunction.Count == 1)
            {
                AttributeSetValueMessage msg = new AttributeSetValueMessage();
                Int32 Id = _changedAttributesQueueForthisFunction.Dequeue();
                AttributeValue value = _attributeValues[Id];

                msg.ActorID = _parent.DynamicID;
                msg.NetAttribute = new NetAttributeKeyValue();
                msg.NetAttribute.Attribute = GameAttributeStaticList.AttributesByID[Id];
                if (msg.NetAttribute.Attribute.IsInteger)
                    msg.NetAttribute.Int = value.Value;
                else
                    msg.NetAttribute.Float = value.ValueF;

                messageList.Add(msg);
            }
            else
            {*/
                while (_changedAttributesQueueForthisFunction.Count >= 15)
                {
                    AttributesSetValuesMessage msg = new AttributesSetValuesMessage();
                    msg.ActorID = owner.DynamicID;
                    msg.atKeyVals = new NetAttributeKeyValue[15];
                    for (int i = 0; i < 15; i++)
                    {
                        Int32 Id = _changedAttributesQueueForthisFunction.Dequeue();
                        AttributeValue value = _currentAttributeMap[Id];
                        msg.atKeyVals[i] = new NetAttributeKeyValue();
                        msg.atKeyVals[i].Attribute = GameAttributeStaticList.AttributesByID[Id];
                        if (msg.atKeyVals[i].Attribute.IsInteger)
                            msg.atKeyVals[i].Int = value.Value;
                        else
                            msg.atKeyVals[i].Float = value.ValueF;

                    }
                    messageList.Add(msg); // Agrega el msg con 15 netattr
                }

                if (_changedAttributesQueueForthisFunction.Count > 0)
                {
                    AttributesSetValuesMessage msg = new AttributesSetValuesMessage();
                    msg.ActorID = owner.DynamicID;
                    msg.atKeyVals = new NetAttributeKeyValue[_changedAttributesQueueForthisFunction.Count];
                    int countleft = _changedAttributesQueueForthisFunction.Count;
                    for (int i = 0; i < countleft; i++)
                    {
                        Int32 Id = _changedAttributesQueueForthisFunction.Dequeue();
                        AttributeValue value = _currentAttributeMap[Id];
                        msg.atKeyVals[i] = new NetAttributeKeyValue();

                        msg.atKeyVals[i].Attribute = GameAttributeStaticList.AttributesByID[Id];
                        if (msg.atKeyVals[i].Attribute.IsInteger)
                            msg.atKeyVals[i].Int = value.Value;
                        else
                            msg.atKeyVals[i].Float = value.ValueF;
                    }
                    messageList.Add(msg); // Agrega el msg con len left (entre 1 y 15)
                }

            //}
            return messageList;
        }

        private List<GameMessage> GetChangedMessageList()
        {
            var messageList = new List<GameMessage>();
            if (_changedAttributesQueue.Count == 0)
                return messageList;
            /*if (_changedAttributesQueue.Count == 1)
            {
                AttributeSetValueMessage msg = new AttributeSetValueMessage();
                Int32 Id = _changedAttributesQueue.Dequeue();
                AttributeValue value = _attributeValues[Id];

                msg.ActorID = _parent.DynamicID;
                msg.NetAttribute = new NetAttributeKeyValue();

                msg.NetAttribute.Attribute = GameAttributeStaticList.AttributesByID[Id];
                if (msg.NetAttribute.Attribute.IsInteger)
                    msg.NetAttribute.Int = value.Value;
                else
                    msg.NetAttribute.Float = value.ValueF;

                messageList.Add(msg);
            }
            else
            {*/
                while (_changedAttributesQueue.Count >= 15)
                {
                    AttributesSetValuesMessage msg = new AttributesSetValuesMessage();
                    msg.ActorID = owner.DynamicID;
                    msg.atKeyVals = new NetAttributeKeyValue[15];
                    for (int i = 0; i < 15; i++)
                    {
                        Int32 Id = _changedAttributesQueue.Dequeue();
                        AttributeValue value = _currentAttributeMap[Id];

                        msg.atKeyVals[i] = new NetAttributeKeyValue();
                        msg.atKeyVals[i].Attribute = GameAttributeStaticList.AttributesByID[Id];
                        if (msg.atKeyVals[i].Attribute.IsInteger)
                            msg.atKeyVals[i].Int = value.Value;
                        else
                            msg.atKeyVals[i].Float = value.ValueF;

                    }
                    messageList.Add(msg); // Agrega el msg con 15 netattr
                }

                if (_changedAttributesQueue.Count > 0)
                {
                    AttributesSetValuesMessage msg = new AttributesSetValuesMessage();
                    msg.ActorID = owner.DynamicID;
                    msg.atKeyVals = new NetAttributeKeyValue[_changedAttributesQueue.Count];
                    int countleft = _changedAttributesQueue.Count;
                    for (int i = 0; i < countleft; i++)
                    {
                        Int32 Id = _changedAttributesQueue.Dequeue();
                        AttributeValue value = _currentAttributeMap[Id];

                        msg.atKeyVals[i] = new NetAttributeKeyValue();
                        msg.atKeyVals[i].Attribute = GameAttributeStaticList.AttributesByID[Id];
                        if (msg.atKeyVals[i].Attribute.IsInteger)
                            msg.atKeyVals[i].Int = value.Value;
                        else
                            msg.atKeyVals[i].Float = value.ValueF;

                    }

                    messageList.Add(msg); // Agrega el msg con len left (entre 1 y 15)
                }


            return messageList;
        }

        private AttributeValue GetAttributeValue(Attribute attribute)
        {
            Int32 Id = attribute.Id;

            AttributeValue gaValue;

            if (_currentAttributeMap.TryGetValue(Id, out gaValue))
                return gaValue;
            return attribute._defaultValue;

        }

        private void SetAttributeValue(Attribute attribute, AttributeValue value)
        {
            if (attribute.EncodingType == AttributeEncoding.IntMinMax)
            {
                if (value.Value < attribute.Min.Value || value.Value > attribute.Max.Value)
                    throw new ArgumentOutOfRangeException("GameAttribute." + attribute.Name.Replace(' ', '_'), "Min: " + attribute.Min.Value + " Max: " + attribute.Max.Value + " Tried to set: " + value.Value);
            }
            if (attribute.EncodingType == AttributeEncoding.Float16)
            {
                if (value.ValueF < Attribute.Float16Min || value.ValueF > Attribute.Float16Max)
                    throw new ArgumentOutOfRangeException("GameAttribute." + attribute.Name.Replace(' ', '_'), "Min: " + Attribute.Float16Min + " Max " + Attribute.Float16Max + " Tried to set: " + value.ValueF);
            }

            Int32 Id = attribute.Id;

            _currentAttributeMap[Id] = value;

            if (!_changedAttributesQueue.Contains(Id))
                _changedAttributesQueue.Enqueue(Id);
        }

        public int this[AttributeI attribute]
        {
            get { return GetAttributeValue(attribute).Value; }
            set { SetAttributeValue(attribute, new AttributeValue(value)); }
        }

        public float this[AttributeF attribute]
        {
            get { return GetAttributeValue(attribute).ValueF; }
            set { SetAttributeValue(attribute, new AttributeValue(value)); }
        }


        public bool this[AttributeB attribute]
        {
            get { return GetAttributeValue(attribute).Value != 0; }
            set { SetAttributeValue(attribute, new AttributeValue(value ? 1 : 0)); }
        }


    }
}
