using System;
using UIKit;
using TeCHeCerchi.Shared.ViewModels;
using TeCHeCerchi.Shared.Interfaces;
using TeCHeCerchi.Shared.Helpers;

namespace TeCHeCerchi.iOS
{
    partial class QuestAskQuestionViewController : UIViewController
    {
        public QuestViewModel ViewModel { get; set; }

        public QuestAskQuestionViewController(IntPtr handle)
            : base(handle)
        {
        }

        partial void ButtonCancel_TouchUpInside(UIButton sender)
        {
            DismissViewControllerAsync(true);
        }

        partial void ButtonAnswer_TouchUpInside(UIButton sender)
        {
            if (ViewModel.QuestComplete)
            {
                DismissViewControllerAsync(true);
                return;
            }

            messages.AskQuestions("Question:", ViewModel.Quest.Question, (answer) =>
                {
                    ViewModel.CheckAnswer(answer);
                    if (ViewModel.QuestComplete)
                    {
                        ButtonCancel.Hidden = true;
                        ButtonAnswer.SetTitle("Continua", UIControlState.Normal);
                        LabelHint.Text = "Pronti per continuare l'avventura con una nuova ricerca ?";
                        LabelAwesome.Text = "Fatto !";
                        LabelCongrats.Text = "Hai risposto correttamente alla domanda, ottimo.";
                        Settings.QuestDone = true;
                    }
                });
      
        }

        private IMessageDialog messages;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            messages = ServiceContainer.Resolve<IMessageDialog>();
            ButtonAnswer.Layer.CornerRadius = ButtonCancel.Layer.CornerRadius = 5;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            LabelQuestNumber.Text = ViewModel.CompletionDisplayShort;
            LabelHint.Text = ViewModel.Quest.Question.Text;
        }
    }
}
