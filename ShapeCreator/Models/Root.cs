using System.Text.Json.Serialization;

namespace ShapeCreator.Models;

public class Root
{
    [JsonPropertyName("shapes")]
    public List<Shape> Shapes { get; set; }
}
