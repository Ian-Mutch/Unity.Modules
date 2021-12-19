using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Modules
{
    class BinarySaveDataUtility : ISaveDataUtility
    {
        public async Task<ReadDataResult> ReadAsync(CancellationToken cancellationToken = default)
        {
            var filePath = $"{Application.dataPath}/player.save";

            if (!Directory.Exists(filePath))
            {
                Debug.Log($"File at path {filePath} does not exist, no data to read");
                return null;
            }

            try
            {
                Debug.Log($"Started reading save data");

                var data = await File.ReadAllBytesAsync(filePath, cancellationToken: cancellationToken);

                using var memorySteam = new MemoryStream(data);
                var formatter = new BinaryFormatter();
                var saveData = (SaveData)formatter.Deserialize(memorySteam);

                Debug.Log($"Successfully read save data from {filePath}");

                return new ReadDataResult(true, saveData);
            }
            catch (SerializationException ex)
            {
                Debug.LogException(ex);
                return new ReadDataResult(false, null);
            }
        }

        public async Task<WriteDataResult> WriteAsync(SaveData saveData, CancellationToken cancellationToken = default)
        {
            try
            {
                Debug.Log($"Started writing data");

                using var memorySteam = new MemoryStream();

                var formatter = new BinaryFormatter();
                formatter.Serialize(memorySteam, saveData);

                var data = memorySteam.GetBuffer();
                memorySteam.Close();

                var filePath = $"{Application.dataPath}/player.save";

                Directory.CreateDirectory(filePath);
                await File.WriteAllBytesAsync(filePath, data, cancellationToken: cancellationToken);

                Debug.Log($"Successfully wrote save data to {filePath}");

                return new WriteDataResult(true);
            }
            catch (SerializationException ex)
            {
                Debug.LogException(ex);
                return new WriteDataResult(false);
            }
        }
    }
}
