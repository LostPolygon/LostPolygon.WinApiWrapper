using System.Collections.Generic;

namespace LostPolygon.NanoWinForms {
    public class ApplicationContext {
        private readonly List<Form>  _forms = new List<Form>();
        private readonly MessagePump _messagePump = new MessagePump();

        public void Run() {
            _messagePump.Run();
        }

        public void Quit() {
            foreach (Form form in _forms.ToArray()) {
                form.Close();
            }

            _messagePump.Quit();
        }

        internal void AddForm(Form form) {
            if (_forms.Contains(form))
                return;

            _forms.Add(form);
        }

        internal void RemoveForm(Form form) {
            _forms.Remove(form);

            if (_forms.Count == 0) {
                Quit();
            }
        }
    }
}
