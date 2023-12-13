using System.Text.Json;
using Out;

try
{
    Console.Error.WriteLine("Executing the out...");

    var standardInputPayload = Console.ReadLine();

    //Console.Error.WriteLine(standardInputPayload);

    ArgumentException.ThrowIfNullOrEmpty(standardInputPayload);

    var concoursePayload = JsonSerializer.Deserialize(standardInputPayload, SourceGenerationContext.Default.ConcoursePayload);

    ArgumentNullException.ThrowIfNull(concoursePayload);
    ArgumentNullException.ThrowIfNull(concoursePayload.Source);
    ArgumentNullException.ThrowIfNull(concoursePayload.Params);
    ArgumentNullException.ThrowIfNull(concoursePayload.Params.File);

    ArgumentException.ThrowIfNullOrEmpty(concoursePayload.Source.AccessKey);
    ArgumentException.ThrowIfNullOrEmpty(concoursePayload.Source.SecretKey);
    ArgumentException.ThrowIfNullOrEmpty(concoursePayload.Source.TableName);
    ArgumentException.ThrowIfNullOrEmpty(concoursePayload.Source.PartitionKey);
    ArgumentException.ThrowIfNullOrEmpty(concoursePayload.Source.Key);

    //Console.Error.WriteLine(args[0]);

    // var directory = args[0];

    // //Console.Error.WriteLine($"The directory is {directory}.");
    // //Console.Error.WriteLine($"The file name is {concoursePayload.Params.File}.");

    // var filePath = Path.Combine(directory, concoursePayload.Params.File);

    var fileText = "Hello!";

    // var fileText = (await File.ReadAllTextAsync(filePath)).Replace(Environment.NewLine, string.Empty);

    // Console.Error.WriteLine(fileText);

    // var accessKey = concoursePayload.Source.AccessKey;
    // var secretKey = concoursePayload.Source.SecretKey;

    // var region = RegionEndpoint.GetBySystemName(concoursePayload.Source.Region);

    // var repository = new Repository(accessKey, secretKey, region);

    // var table        = concoursePayload.Source.TableName;
    // var partitionKey = concoursePayload.Source.PartitionKey;
    // var key          = concoursePayload.Source.Key;

    // var values = await repository.GetAll(table, partitionKey, key);

    // if (values.Any(a => a.Value == fileText))
    // {
    //     // Return the output for an already existing artifact.
    // }

    var created = DateTime.UtcNow;

    // await repository.Create(table, partitionKey, key, fileText, created);

    var output = $"{{\"version\": {{ \"value\": \"{fileText}\", \"created\":\"{created}\"}}}}";

    Console.WriteLine(output);
}
catch (Exception exception)
{
    Console.Error.WriteLine($"{exception.Message}");
}
