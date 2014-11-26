using System;
using System.Windows.Controls;
using ManttoProductosAlternos.DTO;

namespace ManttoProductosAlternos.Reportes
{
    public class AgrRelacionesMes
    {
        Microsoft.Office.Interop.Word.Application oWord;
        Microsoft.Office.Interop.Word.Document oDoc;
        object oMissing = System.Reflection.Missing.Value;
        //readonly object oEndOfDoc = "\\endofdoc";
        //private int numTesis = 0;

        private readonly TreeView treeView = null;

        public AgrRelacionesMes(TreeView treeView)
        {
            this.treeView = treeView;
        }

        public int GeneraWord()
        {
            oWord = new Microsoft.Office.Interop.Word.Application();
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            try
            {
                foreach (TreeViewItem item in treeView.Items)
                {
                    Microsoft.Office.Interop.Word.Paragraph par = oDoc.Content.Paragraphs.Add(ref oMissing);

                    par.Range.Font.Bold = 0;
                    par.Range.Font.Size = 10;
                    par.Range.Font.Name = "Arial";
                    par.Range.Text = item.Header.ToString();
                    par.Range.InsertParagraphAfter();

                    ImprimeDocumento(item);
                }



                /* //Agregando esto para guardar hasta el inicio del catch
                 SaveFileDialog CuadroDialogo = new SaveFileDialog();
                 CuadroDialogo.DefaultExt = "docx";
                 CuadroDialogo.Filter = "docx file(*.docx)|*.docx";
                 CuadroDialogo.AddExtension = true;
                 CuadroDialogo.RestoreDirectory = true;
                 CuadroDialogo.Title = "Guardar";
                 CuadroDialogo.InitialDirectory = @"D:\RESPALDO\SEMANARI\";
                 if (CuadroDialogo.ShowDialog() == DialogResult.OK)
                 {
                     oWord.ActiveDocument.SaveAs(CuadroDialogo.FileName);
                     oWord.ActiveDocument.Saved = true;
                     CuadroDialogo.Dispose();
                     CuadroDialogo = null;
                 }*/
            }
            catch (Exception) { }
            finally
            {
                oWord.Visible = true;
            }
            return 0;
        }

        private void ImprimeDocumento(TreeViewItem item)
        {
            Microsoft.Office.Interop.Word.Paragraph par = oDoc.Content.Paragraphs.Add(ref oMissing);

            foreach (TreeViewItem child in item.Items)
            {
                Temas tema = (Temas)child.Tag;

                par.Range.Font.Bold = 0;
                par.Range.Font.Size = 10;
                par.Range.Font.Name = "Arial";

                if (tema.Nivel == 0)
                    par.Range.Text = tema.Tema;
                else if (tema.Nivel == 1)
                    par.Range.Text = "     " + tema.Tema;
                else if (tema.Nivel == 2)
                    par.Range.Text = "          " + tema.Tema;
                else if (tema.Nivel == 3)
                    par.Range.Text = "               " + tema.Tema;

                par.Range.InsertParagraphAfter();

                ImprimeDocumento(child);

            }
        }

    }
}

