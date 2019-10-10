using Fame.Internal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Fame.Parser {
    public class MSEPrinter {

        protected int indentation;
        protected StringBuilder stream = new StringBuilder();
        public static readonly object UNLIMITED = new Object();
        private bool newLineHasBeenWritten;
        private string filepath;

        public MSEPrinter(string filepath) {
            this.filepath = filepath;
        }

        protected void IndentLine() {
            if (!newLineHasBeenWritten) {
                stream.Append('\n');

                for (int n = 0; n < indentation; n++) {
                    stream.Append('\t');
                }
            }

            newLineHasBeenWritten = true;
        }

        public void BeginAttribute(String name) {
            indentation++;
            IndentLine();
            AppendCharacter('(');
            AppendString(name);
        }

        private void AppendString(string name) {
            stream.Append(name);
            newLineHasBeenWritten = false;
        }

        private void AppendCharacter(char v) {
            stream.Append(v);
            newLineHasBeenWritten = false;
        }

        public void BeginDocument() {
            AppendCharacter('(');
        }
        public void BeginElement(String name) {
            indentation++;
            IndentLine();
            AppendCharacter('(');
            AppendString(name);
        }
        public void EndAttribute(String name) {
            AppendCharacter(')');
            indentation--;
        }
        public void EndDocument() {
            AppendCharacter(')');
            Close();
        }
        private void Close() {
            string s = stream.ToString();
            if (filepath != null)
                System.IO.File.WriteAllText(filepath, stream.ToString());
        }
        public string GetMSE() {
            return stream.ToString();
        }
        public void EndElement(String name) {
            AppendCharacter(')');
            indentation--;
        }

        public void Primitive(Object value) {
            AppendCharacter(' ');
            if (value == UNLIMITED) {
                PrivateDealWithUnlimitedType();
                return; 
            }
            if (value.GetType() == typeof(string)) {
                PrivateDealWithStringType((string)value);
                return;
            } 
            if (value is bool) {
                PrivateDealWithBooleanType((bool)value);
                return;
            } 
            if (Number.IsNumber(value)) {
                PrivateDealWithNumber(value);
                return;
            }
            throw new Exception("Unexpected primitive object type");
        }
        private void PrivateDealWithUnlimitedType() {
            AppendCharacter('*');
        }

        private void PrivateDealWithStringType(string str) {
            AppendCharacter('\'');
            foreach (char ch in str) {
                if (ch == '\'') AppendCharacter('\'');
                AppendCharacter(ch);
            }
            AppendCharacter('\'');
        }
        private void PrivateDealWithBooleanType(bool value) {
            AppendString(value.ToString().ToLower());
        }
        private void PrivateDealWithNumber(object value) {
            AppendString(value.ToString());
        }
        public void Reference(int index) {
            stream.Append(" (ref: "); // must prepend space!
            stream.Append(index);
            stream.Append(')');
        }

        public void Reference(String name) {
            stream.Append(" (ref: "); // must prepend space!
            stream.Append(name);
            stream.Append(')');
        }

        public void Serial(int index) {
            stream.Append(" (id: "); // must prepend space!
            stream.Append(index);
            stream.Append(')');
        }

    }
}
