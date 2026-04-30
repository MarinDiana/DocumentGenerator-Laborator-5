# Document Generator – Design Patterns

Acest proiect demonstreaza utilizarea principalelor pattern-uri creationale in C#, intr-un sistem de generare documente (rapoarte si facturi).

Aplicatia permite exportul documentelor in formate diferite si configurarea flexibila a continutului folosind mai multe pattern-uri.

---

## 1. Factory Method

**Problema:**
Codul client nu trebuie sa depinda de clase concrete pentru randare (HTML sau Plain Text).

**Solutie:**
Se defineste o clasa abstracta `DocumentExporter` care contine metoda `CreateRenderer()`.
Subclasele (`HtmlDocumentExporter`, `PlainTextDocumentExporter`) decid ce renderer concret este creat.

**Unde in cod:**
- `DocumentExporter`
- `HtmlDocumentExporter`
- `PlainTextDocumentExporter`
- `IDocumentRenderer`

---

## 2. Abstract Factory

**Problema:**
Trebuie create familii de componente compatibile (header, section, footer) pentru tipuri diferite de documente (raport / factura).

**Solutie:**
Se defineste interfata `IDocumentComponentFactory` care creeaza componentele documentului.
Implementarile (`ReportComponentFactory`, `InvoiceComponentFactory`) genereaza familii diferite.

**Unde in cod:**
- `IDocumentComponentFactory`
- `ReportComponentFactory`
- `InvoiceComponentFactory`
- `DocumentAssembler`

---

## 3. Builder

**Problema:**
Crearea obiectelor complexe (DocumentData) devine greu de gestionat prin constructori.

**Solutie:**
Se foloseste `DocumentDataBuilder` pentru a construi obiectul pas cu pas folosind metode fluente.

**Unde in cod:**
- `DocumentDataBuilder`
- `DocumentData`

**Validare:**
Metoda `Build()` verifica:
- existenta titlului
- existenta autorului
- existenta a cel putin unei sectiuni

---

## 4. Prototype

**Problema:**
Crearea repetata a unor obiecte similare este costisitoare.

**Solutie:**
Se foloseste clonarea obiectelor existente (deep copy).
`TemplateRegistry` stocheaza sabloane si returneaza clone independente.

**Unde in cod:**
- `DocumentTemplate`
- `TemplateRegistry`
- `PageLayout`

---

## 5. Singleton

**Problema:**
Configuratia aplicatiei trebuie sa fie unica si accesibila global.

**Solutie:**
Clasa `AppConfiguration` foloseste `Lazy<T>` pentru a asigura o singura instanta.

**Unde in cod:**
- `AppConfiguration`

---

## 6. Testare

Sunt implementate teste unitare folosind MSTest pentru verificarea functionalitatii:

- Factory Method → output diferit pentru formate diferite
- Abstract Factory → componente diferite pentru raport si factura
- Builder → validare si aruncare exceptii
- Prototype → clone independente

**Unde in cod:**
- proiectul `DocumentGenerator.Tests`

---

## Concluzie

Proiectul demonstreaza utilizarea corecta a pattern-urilor creationale pentru:
- decuplarea codului
- flexibilitate
- reutilizare
- testabilitate
