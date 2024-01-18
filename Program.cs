using System.Globalization;

using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

using TP2_TSV;
using TP2_TSV.Actors;

// Path of files
string mediaPath = "Dataset\\medias_francais.tsv";
string relationPath = "Dataset\\relations_medias_francais.tsv";
string delimiter = "\t";
string lineDelimiter = "\n";

var tsvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
{
    Delimiter = delimiter,
    NewLine = lineDelimiter,
    HasHeaderRecord = true,
    PrepareHeaderForMatch = args => args.Header.ToLower(),
};

List<IActor> actors = new List<IActor>();


// Load the TSV media file
await LoadMediaFile(mediaPath, tsvConfig, actors);

List<Graph> graphs = new();

// Load the TSV relation file
await LoadRelationFile(relationPath, tsvConfig, actors, graphs);

// Search for Télérama in the graph
var res = FindInGraph("Télérama", graphs);
Console.WriteLine("Télérama est possédé à :");
foreach (var graph in res)
{
    Console.WriteLine($"{graph}% par {graph.Element.Nom} ({graph.Element.TypeLibelle})");
}

static List<Graph> FindInGraph(string elementName, List<Graph> graphs)
{
    List<Graph> actors = new();

    Graph root = graphs.Find(g => g.Childs.Any(c => c.Key.Element.Nom == elementName));
    if (root == null)
    {
        Console.WriteLine($"The element {elementName} has not been found");
        return actors;
    }

    actors.AddRange(FindInGraph(root, graphs));

    static List<Graph> FindInGraph(Graph graph, List<Graph> graphs)
    {
        List<Graph> actors = new();

        if (graph.Childs == null || graph.Childs.Count <= 0)
        {
            actors.Add(graph);
            return actors;
        }
        else
        {
            foreach (var child in graph.Childs)
                actors.AddRange(FindInGraph(child.Key, graphs));
        }
        return actors;
    }

    return actors;
}




static async Task LoadMediaFile(string mediaPath, CsvConfiguration tsvConfig, List<IActor> actors)
{
    using (var reader = new StreamReader(mediaPath))
    using (var csv = new CsvReader(reader, tsvConfig))
    {
        Console.WriteLine($"Reading file {mediaPath}...");
        while (await csv.ReadAsync())
        {
            //Switch the column media_type create a MoralPerson, Media or PhysicalPerson
            csv.ReadHeader();
            var mediaType = csv[2];//"typeLibelle"];
            mediaType = mediaType.ToLower().Trim();
            IActor t = null;
            switch (mediaType)
            {
                case "personne physique":
                    t = csv.GetRecord<PhysicalPerson>();
                    actors.Add(t);
                    break;
                case "personne moral":
                    t = csv.GetRecord<MoralPerson>();
                    actors.Add(t);
                    break;
                case "média":
                    t = csv.GetRecord<Media>();
                    actors.Add(t);
                    break;
            }

            Console.WriteLine(t);
        }
    }
}

static async Task LoadRelationFile(string relationPath, CsvConfiguration tsvConfig, List<IActor> actors, List<Graph> graphs)
{
    using (var reader = new StreamReader(relationPath))
    using (var csv = new CsvReader(reader, tsvConfig))
    {
        Console.WriteLine($"Reading file {relationPath}...");
        while (await csv.ReadAsync())
        {
            //csv.ReadHeader();

            // Depending of the relation create or alter the corresponding Graph element
            TOTO t = csv.GetRecord<TOTO>();

            Graph alreadyExist = graphs.Find(g => g.Element.Nom == t.Origine);

            // Find the Actor in the list
            IActor element = actors.Find(a => a.Nom == t.Origine);
            if (element == null)
                continue;

            // Get the value
            if (t.Valeur == "contrôle")
                t.Valeur = "66";
            else if (t.Valeur == "participe")
                t.Valeur = "33";
            else if (t.Valeur == ">50")
                t.Valeur = "51";

            Console.WriteLine("value = " + t.Valeur);
            float f = float.Parse(t.Valeur);
            Console.WriteLine("value = " + f);

            // Get the child
            IActor child = actors.Find(a => a.Nom == t.Cible);
            if (alreadyExist == null)
            {

                Graph graph = new(element);
                if (child != null)
                    graph.Childs.Add(new(child), f);
            }
            else
            {
                if (child != null)
                    alreadyExist.Childs.Add(new(child), f);

                Console.WriteLine(alreadyExist);

            }

        }
    }
}

class TOTO
{
    [Index(0)]
    public string Id { get; set; }

    [Index(1)]
    public string Origine { get; set; }

    [Index(2)]
    public string Valeur { get; set; }

    [Index(3)]
    public string Cible { get; set; }

    [Index(4)]
    public string Source { get; set; }

    [Index(5)]
    public string DatePublication { get; set; }

    [Index(6)]
    public string DateConsultation { get; set; }
};