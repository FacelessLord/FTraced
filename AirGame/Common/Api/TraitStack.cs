using System.Collections.Generic;
using System.Net.Json;
using GlLib.Utils;

namespace GlLib.Common.Api
{
    public class TraitStack : IJsonSerializable
    {
        public float baseValue;

        public List<TraitModifier> modifiers;
        public Trait trait;

        public TraitStack()
        {
            trait = Trait.Health;
            baseValue = 0;
        }

        public TraitStack(Trait _trait)
        {
            trait = _trait;
            baseValue = _trait.baseValue;
        }

        public TraitStack(Trait _trait, float _baseValue)
        {
            trait = _trait;
            baseValue = _baseValue;
        }

        public JsonObject Serialize(string _objectName)
        {
            var jsonObj = new JsonObjectCollection(_objectName);
            jsonObj.Add(new JsonNumericValue("traitId", trait.id));
            jsonObj.Add(new JsonNumericValue("baseValue", baseValue));
            if (modifiers != null && modifiers.Count != 0) jsonObj.Add(JsonHelper.SaveList("modifiers", modifiers));

            return jsonObj;
        }

        public void Deserialize(JsonObject _jsonObject)
        {
            if (_jsonObject is JsonObjectCollection collection)
            {
                trait = Trait.traits[(int) ((JsonNumericValue) collection[0]).Value];
                baseValue = (float) ((JsonNumericValue) collection[1]).Value;
                modifiers = JsonHelper.LoadList<TraitModifier>((JsonArrayCollection) collection[2]);
            }
        }

        public string GetStandardName()
        {
            return "traitStack";
        }

        public void ApplyModifier(TraitModifier _tm)
        {
            if (modifiers == null) modifiers = new List<TraitModifier>();

            if (modifiers.Contains(_tm)) return;

            modifiers.Add(_tm);
        }

        public void RemoveModifier(TraitModifier _tm)
        {
            if (modifiers != null)
                if (modifiers.Contains(_tm))
                    modifiers.Remove(_tm);
        }

        /**
         * Removes all modifiers with id = given id
         * 
         * @param id
         */
        public void RemoveModifier(string _uuid)
        {
            if (modifiers != null)
            {
                var rmList = new List<TraitModifier>();
                foreach (var tm in modifiers)
                    if (tm.uuid == _uuid)
                        rmList.Add(tm);

                modifiers.RemoveAll(_tm => rmList.Contains(_tm));
            }
        }

        public float CountValue()
        {
            float add = 0;
            float perc = 0;
            float mult = 1;
            if (modifiers != null)
                foreach (var tm in modifiers)
                    switch (tm.operation)
                    {
                        case TraitModifier.Operation.Add:
                            add += tm.value;
                            break;
                        case TraitModifier.Operation.AddPercent:
                            perc += tm.value;
                            break;
                        case TraitModifier.Operation.Mult:
                            mult *= tm.value;
                            break;
                    }

            return (baseValue + add) * (1 + perc) * mult;
        }

        public TraitStack Copy()
        {
            var ret = new TraitStack(trait, baseValue);
            ret.modifiers = modifiers;
            return ret;
        }
    }
}