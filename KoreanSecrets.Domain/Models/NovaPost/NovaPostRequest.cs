using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Models.NovaPost;

public class NovaPostRequest
{
    public string apiKey { get; set; }
    public string modelName { get; set; }
    public string calledMethod { get; set; }
    public MethodProperties methodProperties { get; set; }

    public NovaPostRequest(string apiKey, string modelName, string calledMethod)
    {
        this.apiKey = apiKey;
        this.modelName = modelName;
        this.calledMethod = calledMethod;
    }
}
