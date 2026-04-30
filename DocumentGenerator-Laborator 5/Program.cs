using System;
using System.Collections.Generic;
using System.Text;
using static DocumentDataBuilder;

public class DocumentData
{
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime Date { get; set; }
    public List<string> Sections { get; set; }
    public string PageFormat { get; set; }
    public string Orientation { get; set; }
    public string FooterNote { get; set; }

    public DocumentData()
    {
        Sections = new List<string>();
        Date = DateTime.Now;
        PageFormat = "A4";
        Orientation = "Portrait";
        FooterNote = "";
    }
}

public interface IDocumentRenderer
{
    string Render(DocumentData data);
}

public class HtmlDocumentRenderer : IDocumentRenderer
{
    public string Render(DocumentData data)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("<html>");
        sb.AppendLine("<body>");
        sb.AppendLine("<h1>" + data.Title + "</h1>");
        sb.AppendLine("<p>Autor: " + data.Author + "</p>");

        foreach (string section in data.Sections)
        {
            sb.AppendLine("<p>" + section + "</p>");
        }

        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

        return sb.ToString();
    }
}

public class PlainTextDocumentRenderer : IDocumentRenderer
{
    public string Render(DocumentData data)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Titlu: " + data.Title);
        sb.AppendLine("Autor: " + data.Author);
        sb.AppendLine();

        foreach (string section in data.Sections)
        {
            sb.AppendLine(section);
        }

        return sb.ToString();
    }
}

public abstract class DocumentExporter
{
    protected abstract IDocumentRenderer CreateRenderer();

    public string Export(DocumentData data)
    {
        IDocumentRenderer renderer = CreateRenderer();
        return renderer.Render(data);
    }
}

public class HtmlDocumentExporter : DocumentExporter
{
    protected override IDocumentRenderer CreateRenderer()
    {
        return new HtmlDocumentRenderer();
    }
}

public class PlainTextDocumentExporter : DocumentExporter
{
    protected override IDocumentRenderer CreateRenderer()
    {
        return new PlainTextDocumentRenderer();
    }
}

public class DocumentExportService
{
    private readonly DocumentExporter exporter;

    public DocumentExportService(DocumentExporter exporter)
    {
        this.exporter = exporter;
    }

    public void ExportDocument(DocumentData data)
    {
        string rezultat = exporter.Export(data);
        Console.WriteLine(rezultat);
    }
}

public interface IDocumentComponentFactory
{
    string CreateHeader();
    string CreateSection();
    string CreateFooter();
}

public class ReportComponentFactory : IDocumentComponentFactory
{
    public string CreateHeader()
    {
        return "RAPORT - Logo firma | Numar raport: R-001";
    }

    public string CreateSection()
    {
        return "Sectiune raport: Tabel cu date, statistici si observatii.";
    }

    public string CreateFooter()
    {
        return "Subsol raport: Semnatura responsabil raport.";
    }
}

public class InvoiceComponentFactory : IDocumentComponentFactory
{
    public string CreateHeader()
    {
        return "FACTURA - Date fiscale firma | Cod fiscal: RO123456";
    }

    public string CreateSection()
    {
        return "Sectiune factura: Linie factura - produs, cantitate, pret.";
    }

    public string CreateFooter()
    {
        return "Subsol factura: Total de plata si termen scadent.";
    }
}

public class DocumentAssembler
{
    private readonly IDocumentComponentFactory factory;

    public DocumentAssembler(IDocumentComponentFactory factory)
    {
        this.factory = factory;
    }

    public void AssembleDocument()
    {
        Console.WriteLine(factory.CreateHeader());
        Console.WriteLine(factory.CreateSection());
        Console.WriteLine(factory.CreateFooter());
    }
}

public class DocumentDataBuilder
{
    private DocumentData document;

    public DocumentDataBuilder()
    {
        document = new DocumentData();
    }

    public DocumentDataBuilder WithTitle(string title)
    {
        document.Title = title;
        return this;
    }

    public DocumentDataBuilder ByAuthor(string author)
    {
        document.Author = author;
        return this;
    }

    public DocumentDataBuilder WithSection(string section)
    {
        document.Sections.Add(section);
        return this;
    }

    public DocumentDataBuilder InLandscape()
    {
        document.Orientation = "Landscape";
        return this;
    }

    public DocumentDataBuilder WithFooterNote(string footerNote)
    {
        document.FooterNote = footerNote;
        return this;
    }

    public DocumentData Build()
    {
        if (string.IsNullOrWhiteSpace(document.Title))
        {
            throw new InvalidOperationException("Documentul trebuie sa aiba titlu.");
        }

        if (string.IsNullOrWhiteSpace(document.Author))
        {
            throw new InvalidOperationException("Documentul trebuie sa aiba autor.");
        }

        if (document.Sections.Count == 0)
        {
            throw new InvalidOperationException("Documentul trebuie sa aiba cel putin o sectiune.");
        }

        return document;
    }

    public class PageLayout
    {
        public string PageFormat { get; set; }
        public string Orientation { get; set; }

        public PageLayout()
        {
            PageFormat = "A4";
            Orientation = "Portrait";
        }

        public PageLayout Clone()
        {
            return new PageLayout
            {
                PageFormat = this.PageFormat,
                Orientation = this.Orientation
            };
        }
    }

    public class DocumentTemplate : ICloneable
    {
        public string Title { get; set; }
        public List<string> Sections { get; set; }
        public PageLayout Layout { get; set; }

        public DocumentTemplate()
        {
            Title = "";
            Sections = new List<string>();
            Layout = new PageLayout();
        }

        public object Clone()
        {
            return new DocumentTemplate
            {
                Title = this.Title,
                Sections = new List<string>(this.Sections),
                Layout = this.Layout.Clone()
            };
        }
    }

    public class TemplateRegistry
    {
        private readonly Dictionary<string, DocumentTemplate> prototypes;

        public TemplateRegistry()
        {
            prototypes = new Dictionary<string, DocumentTemplate>();
        }

        public void Register(string key, DocumentTemplate template)
        {
            prototypes[key] = template;
        }

        public DocumentTemplate Get(string key)
        {
            if (!prototypes.ContainsKey(key))
            {
                throw new KeyNotFoundException("Prototipul nu exista: " + key);
            }

            return (DocumentTemplate)prototypes[key].Clone();
        }
    }

    public class AppConfiguration
    {
        private static readonly Lazy<AppConfiguration> instance =
            new Lazy<AppConfiguration>(() => new AppConfiguration());

        public static AppConfiguration Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public string OutputDirectory { get; private set; }
        public string DefaultFormat { get; private set; }
        public string ApplicationName { get; private set; }

        private AppConfiguration()
        {
            OutputDirectory = "C:\\DocumenteGenerate";
            DefaultFormat = "HTML";
            ApplicationName = "Sistem de Generare Documente";
        }

        public void ShowConfiguration()
        {
            Console.WriteLine("Director iesire: " + OutputDirectory);
            Console.WriteLine("Format implicit: " + DefaultFormat);
            Console.WriteLine("Aplicatie: " + ApplicationName);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        DocumentData document = new DocumentDataBuilder()
    .WithTitle("Raport vanzari")
    .ByAuthor("Marin Diana")
    .WithSection("Sectiunea 1: Introducere")
    .WithSection("Sectiunea 2: Date raport")
    .WithSection("Sectiunea 3: Concluzii")
    .InLandscape()
    .WithFooterNote("Document generat pentru Laborator 5")
    .Build();

        Console.WriteLine("EXPORT HTML:");
        DocumentExporter htmlExporter = new HtmlDocumentExporter();
        DocumentExportService htmlService = new DocumentExportService(htmlExporter);
        htmlService.ExportDocument(document);

        Console.WriteLine();

        Console.WriteLine("EXPORT PLAIN TEXT:");
        DocumentExporter textExporter = new PlainTextDocumentExporter();
        DocumentExportService textService = new DocumentExportService(textExporter);
        textService.ExportDocument(document);

        Console.WriteLine();
        Console.WriteLine("DOCUMENT RAPORT - ABSTRACT FACTORY:");
        IDocumentComponentFactory reportFactory = new ReportComponentFactory();
        DocumentAssembler reportAssembler = new DocumentAssembler(reportFactory);
        reportAssembler.AssembleDocument();

        Console.WriteLine();

        Console.WriteLine("DOCUMENT FACTURA - ABSTRACT FACTORY:");
        IDocumentComponentFactory invoiceFactory = new InvoiceComponentFactory();
        DocumentAssembler invoiceAssembler = new DocumentAssembler(invoiceFactory);
        invoiceAssembler.AssembleDocument();

        Console.WriteLine();

        Console.WriteLine("PROTOTYPE - REGISTRU DE SABLOANE:");

        DocumentTemplate raportTemplate = new DocumentTemplate();
        raportTemplate.Title = "Sablon raport";
        raportTemplate.Sections.Add("Introducere raport");
        raportTemplate.Sections.Add("Tabel cu date");
        raportTemplate.Layout.PageFormat = "A4";
        raportTemplate.Layout.Orientation = "Portrait";

        TemplateRegistry registry = new TemplateRegistry();
        registry.Register("raport", raportTemplate);

        DocumentTemplate raportClonat = registry.Get("raport");

        raportClonat.Title = "Raport clonat si modificat";
        raportClonat.Sections.Add("Concluzie adaugata doar in clona");
        raportClonat.Layout.Orientation = "Landscape";

        Console.WriteLine("SABLON ORIGINAL:");
        Console.WriteLine("Titlu: " + raportTemplate.Title);
        Console.WriteLine("Sectiuni: " + string.Join(", ", raportTemplate.Sections));
        Console.WriteLine("Orientare: " + raportTemplate.Layout.Orientation);

        Console.WriteLine();

        Console.WriteLine("CLONA MODIFICATA:");
        Console.WriteLine("Titlu: " + raportClonat.Title);
        Console.WriteLine("Sectiuni: " + string.Join(", ", raportClonat.Sections));
        Console.WriteLine("Orientare: " + raportClonat.Layout.Orientation);

        Console.WriteLine();

        Console.WriteLine("SINGLETON - CONFIGURATIE GLOBALA:");

        AppConfiguration config1 = AppConfiguration.Instance;
        AppConfiguration config2 = AppConfiguration.Instance;

        config1.ShowConfiguration();

        Console.WriteLine();

        if (object.ReferenceEquals(config1, config2))
        {
            Console.WriteLine("config1 si config2 sunt aceeasi instanta Singleton.");
        }
        else
        {
            Console.WriteLine("config1 si config2 sunt instante diferite.");
        }
    }
}