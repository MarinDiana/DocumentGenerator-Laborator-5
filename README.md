# Document Generator – Design Patterns

Acest proiect implementeaza principalele pattern-uri creationale in C#.

## 1. Factory Method
Permite crearea obiectelor fara a specifica clasa concreta.
Exemplu: HtmlDocumentExporter si PlainTextDocumentExporter.

## 2. Abstract Factory
Creeaza familii de obiecte compatibile.
Exemplu: ReportComponentFactory si InvoiceComponentFactory.

## 3. Builder
Construieste obiecte pas cu pas.
Exemplu: DocumentDataBuilder.

## 4. Prototype
Cloneaza obiecte existente.
Exemplu: DocumentTemplate si TemplateRegistry.

## 5. Singleton
Asigura o singura instanta globala.
Exemplu: AppConfiguration.

## 6. Testare
Teste unitare implementate pentru:
- Factory Method
- Abstract Factory
- Builder
- Prototype
