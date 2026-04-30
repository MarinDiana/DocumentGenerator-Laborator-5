using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static DocumentDataBuilder;

namespace DocumentGenerator.Tests
{
    [TestClass]
    public class Test1
    {
        [TestMethod]
        public void FactoryMethod_ShouldReturnDifferentOutputs()
        {
            DocumentData data = new DocumentDataBuilder()
                .WithTitle("Test")
                .ByAuthor("Autor")
                .WithSection("Sectiune")
                .Build();

            DocumentExporter htmlExporter = new HtmlDocumentExporter();
            DocumentExporter textExporter = new PlainTextDocumentExporter();

            string html = htmlExporter.Export(data);
            string text = textExporter.Export(data);

            Assert.AreNotEqual(html, text);
        }

        [TestMethod]
        public void AbstractFactory_ShouldProduceDifferentHeaders()
        {
            IDocumentComponentFactory reportFactory = new ReportComponentFactory();
            IDocumentComponentFactory invoiceFactory = new InvoiceComponentFactory();

            string reportHeader = reportFactory.CreateHeader();
            string invoiceHeader = invoiceFactory.CreateHeader();

            Assert.AreNotEqual(reportHeader, invoiceHeader);
        }

        [TestMethod]
        public void Builder_ShouldThrowException_WhenTitleMissing()
        {
            try
            {
                new DocumentDataBuilder()
                    .ByAuthor("Autor")
                    .WithSection("Sectiune")
                    .Build();

                Assert.Fail("Trebuia sa arunce InvalidOperationException.");
            }
            catch (InvalidOperationException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void Prototype_ShouldCreateIndependentClones()
        {
            DocumentTemplate template = new DocumentTemplate();
            template.Title = "Original";
            template.Sections.Add("Sec1");

            TemplateRegistry registry = new TemplateRegistry();
            registry.Register("doc", template);

            DocumentTemplate clone1 = registry.Get("doc");
            DocumentTemplate clone2 = registry.Get("doc");

            clone1.Title = "Modificat";

            Assert.AreNotEqual(clone1.Title, clone2.Title);
        }
    }
}