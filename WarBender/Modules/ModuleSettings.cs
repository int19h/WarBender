using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace WarBender.Modules {
    public class ModuleSettings : IReadOnlyDictionary<string, string> {
        private readonly Dictionary<string, string> _settings =
            new Dictionary<string, string>();

        public bool DontLoadRegularTroopInventories =>
            int.Parse(_settings["dont_load_regular_troop_inventories"]) != 0;

        public ModuleSettings(TextReader reader) {
            Trace.WriteLine($"Loading {reader.FileName()}", nameof(ModuleSettings));

            var lineReader = new LineReader(reader);
            string line;
            while ((line = lineReader.ReadLine()) != null) {
                var s = line;
                int comment = s.IndexOf('#');
                if (comment >= 0) {
                    s = s.Substring(0, comment);
                }

                s = s.Trim();
                if (s == "") {
                    continue;
                }

                var pair = s.Split(new[] { '=' }, 2);
                if (pair.Length != 2) {
                    lineReader.InvalidData("Equals sign expected between name and value");
                }

                _settings[pair[0].Trim()] = pair[1].Trim();
            }
        }

        public string this[string key] =>
            _settings[key];

        public IEnumerable<string> Keys =>
            ((IReadOnlyDictionary<string, string>)_settings).Keys;

        public IEnumerable<string> Values =>
            ((IReadOnlyDictionary<string, string>)_settings).Values;

        public int Count =>
            _settings.Count;

        public bool ContainsKey(string key) =>
            _settings.ContainsKey(key);

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() =>
            ((IReadOnlyDictionary<string, string>)_settings).GetEnumerator();

        public bool TryGetValue(string key, out string value) =>
            _settings.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() =>
            ((IReadOnlyDictionary<string, string>)_settings).GetEnumerator();
    }
}
