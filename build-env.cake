#addin nuget:?package=Cake.Docker&version=1.1.2
#addin nuget:?package=dotenv.net&version=3.1.2
var target = Argument("target", "Docker information");
var envVars = dotenv.net.DotEnv.Read();
string [] dockerTags = new string[]  {  $"api_local"};
string containerPort = "4567";

Task("Build Docker Image")
.WithCriteria(() => envVars.ContainsKey("USERNAME"), ".env file need a USERNAME entry")
.WithCriteria(() => envVars.ContainsKey("APIKEY"), ".env file need a APIKEY entry")
.Does(() => {
    string [] arguments = new string[]  {  $"USERNAME={envVars["USERNAME"]}", $"APIKEY={envVars["APIKEY"]}"};
    Information("Building : Docker Image");
    var settings = new DockerImageBuildSettings { 
        Tag=dockerTags,
        BuildArg = arguments
    };
    DockerBuild(settings, ".");

});

Task("Docker information").IsDependentOn("Build Docker Image").Does(()=>{
    Information($"Run the image by running:");
    foreach (var dockerTag in dockerTags)
    {
        Information($"\tdocker run -it -p {containerPort}:80 {dockerTag}");
    }    
});

RunTarget(target);