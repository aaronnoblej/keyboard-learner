using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KeyboardLearner
{
    public abstract partial class NavigationScreen : UserControl
    {
        // PROPERTIES
        protected UserControl _previous;
        protected FlowLayoutPanel _contentContainer = new FlowLayoutPanel();
        //bodyContainer
        //titleText

        // CONSTRUCTORS
        public NavigationScreen(UserControl previous, string title = "")
        {
            this._previous = previous;
            InitializeComponent();
            titleText.Text = title;
            ConfigureFlowLayoutContainer();
            //Back button
            this.backButton.Cursor = Cursors.Hand;
            this.backButton.Click += BackButton_Click;
        }

        /// <summary>
        /// Configures the FlowLayoutContainer used for centering components.
        /// </summary>
        private void ConfigureFlowLayoutContainer()
        {
            this._contentContainer.AutoSize = true;
            this._contentContainer.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this._contentContainer.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom);
            this._contentContainer.AutoScroll = true;
            this._contentContainer.FlowDirection = FlowDirection.TopDown;
            bodyContainer.Controls.Add(this._contentContainer);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Close();
            this._previous.BringToFront();
        }

        protected void Close()
        {
            try
            {
                this._previous.Controls.Remove(this);
            }
            catch(NullReferenceException)
            {
                this.Parent.Controls.Remove(this);
            }
            this.Dispose();
        }
    }
}
