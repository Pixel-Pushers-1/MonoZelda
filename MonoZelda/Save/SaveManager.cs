using MonoZelda.Dungeons;
using MonoZelda.Scenes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonoZelda.Save
{
    public class SaveManager
    {
        private JsonSerializerOptions seralizerSettings => new () { IncludeFields = true, WriteIndented = true };

        public ISaveable Saveable { get; set; }

        public SaveManager(ISaveable saveable)
        {
            Saveable = saveable;
        }

        public void Save()
        {
            var save = new SaveState();

            Saveable.Save(save);

            var saveData = JsonSerializer.Serialize(save, seralizerSettings);
            var savePath = GetSavePath();

            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(savePath)))
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(savePath));

            System.IO.File.WriteAllText(savePath, saveData);
        }

        public void Load()
        {
            var savePath = GetSavePath();

            if (System.IO.File.Exists(savePath))
            {
                var saveData = System.IO.File.ReadAllText(savePath);
                var save = JsonSerializer.Deserialize<SaveState>(saveData, seralizerSettings);

                Saveable.Load(save);
            }
        }

        private string GetSavePath()
        {
            return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MonoZelda", "QuickSave.json");
        }
    }
}
