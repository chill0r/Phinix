using RimWorld;
using Trading;
using UnityEngine;
using Utils;
using Verse;
using static PhinixClient.Client;

namespace PhinixClient
{
    public class TradeWindow : Window
    {
        public override Vector2 InitialSize => new Vector2(1000f, 750f);
        
        private const float DEFAULT_SPACING = 10f;
        private const float SCROLLBAR_WIDTH = 16f;
        private const float WINDOW_PADDING = 20f;

        private const float TRADE_ARROWS_WIDTH = 140f;
        private const float TRADE_ARROWS_HEIGHT = 170f;

        private const float OFFER_WINDOW_WIDTH = 400f;
        private const float OFFER_WINDOW_HEIGHT = 310f;
        private const float OFFER_WINDOW_TITLE_HEIGHT = 20f;
        private const float OFFER_CONFIRMATION_HEIGHT = 20f;

        private const float TRADE_BUTTON_WIDTH = 140f;
        private const float TRADE_BUTTON_HEIGHT = 30f;

        private const float TITLE_HEIGHT = 30f;

        /// <summary>
        /// The ID of the trade this window is for.
        /// </summary>
        private string tradeId;

        /// <summary>
        /// Whether we accept the trade as it stands.
        /// </summary>
        private bool tradeAccepted = false;

        /// <summary>
        /// Creates a new <c>TradeWindow</c> for the given trade ID.
        /// </summary>
        /// <param name="tradeId">Trade ID</param>
        public TradeWindow(string tradeId)
        {
            this.tradeId = tradeId;

            this.doCloseX = true;
            this.closeOnAccept = false;
            this.closeOnCancel = false;
            this.closeOnClickedOutside = false;

            Instance.OnTradeCompleted += OnTradeCompleted;
            Instance.OnTradeCancelled += OnTradeCancelled;
        }

        public override void Close(bool doCloseSound = true)
        {
            base.Close(doCloseSound);
            
            Instance.OnTradeCompleted -= OnTradeCompleted;
            Instance.OnTradeCancelled -= OnTradeCancelled;
        }

        public override void DoWindowContents(Rect inRect)
        {
            // Title
            Rect titleRect = new Rect(
                x: inRect.xMin,
                y: inRect.yMin,
                width: inRect.width,
                height: TITLE_HEIGHT
            );
            DrawTitle(titleRect);
            
            // Offers
            Rect offersRect = new Rect(
                x: inRect.xMin,
                y: titleRect.yMax,
                width: inRect.width,
                height: OFFER_WINDOW_HEIGHT + DEFAULT_SPACING + OFFER_CONFIRMATION_HEIGHT
            );
            DrawOffers(offersRect);
        }
        
        /// <summary>
        /// Event handler for the <c>OnTradeCancelled</c> event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTradeCancelled(object sender, CompleteTradeEventArgs args)
        {
            // Try get the other party's display name
            if (Instance.TryGetDisplayName(args.OtherPartyUuid, out string displayName))
            {
                // Strip formatting
                displayName = TextHelper.StripRichText(displayName);
            }
            else
            {
                // Unknown display name, default to ???
                displayName = "???";
            }

            LetterDef letterDef = new LetterDef {color = Color.yellow};
            Find.LetterStack.ReceiveLetter("Trade cancelled", string.Format("The trade with {0} was cancelled", displayName), letterDef);
            
            Close();
        }

        /// <summary>
        /// Event handler for the <c>OnTradeCompleted</c> event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTradeCompleted(object sender, CompleteTradeEventArgs args)
        {
            // Try get the other party's display name
            if (Instance.TryGetDisplayName(args.OtherPartyUuid, out string displayName))
            {
                // Strip formatting
                displayName = TextHelper.StripRichText(displayName);
            }
            else
            {
                // Unknown display name, default to ???
                displayName = "???";
            }
            
            LetterDef letterDef = new LetterDef {color = Color.cyan};
            Find.LetterStack.ReceiveLetter("Trade success", displayName, letterDef);
            
            Close();
        }

        /// <summary>
        /// Draws the title of the trade window within the given container.
        /// </summary>
        /// <param name="container">Container to draw within</param>
        private void DrawTitle(Rect container)
        {
            // Get the other party's display name
            string displayName = GetOtherPartyDisplayName();
            
            // Set the text style
            Text.Anchor = TextAnchor.MiddleCenter;
            Text.Font = GameFont.Medium;
            
            // Draw the title
            Widgets.Label(container, "Trade with " + TextHelper.StripRichText(displayName));
            
            // Reset the text style
            Text.Anchor = TextAnchor.UpperLeft;
            Text.Font = GameFont.Small;
        }
        
        /// <summary>
        /// Gets the display name of the other party of this trade.
        /// Defaults to '???' if any part of the process fails.
        /// </summary>
        /// <returns>Other party's display name</returns>
        private string GetOtherPartyDisplayName()
        {
            // Try to get the other party's UUID and display name
            string displayName;
            if (!Instance.TryGetOtherPartyUuid(tradeId, out string otherPartyUuid))
            {
                displayName = "???";
            }
            if (!Instance.TryGetDisplayName(otherPartyUuid, out displayName))
            {
                displayName = "???";
            }

            return displayName;
        }
        
        /// <summary>
        /// Draws the offer windows and confirmation statuses within the given container.
        /// </summary>
        /// <param name="container">Container to draw within</param>
        private void DrawOffers(Rect container)
        {
            // Our offer
            Rect ourOfferRect = new Rect(
                x: container.xMin,
                y: container.yMin,
                width: OFFER_WINDOW_WIDTH,
                height: OFFER_WINDOW_HEIGHT
            );
            DrawOurOffer(ourOfferRect);
            
            // Their offer
            Rect theirOfferRect = new Rect(
                x: container.xMax - OFFER_WINDOW_WIDTH,
                y: container.yMin,
                width: OFFER_WINDOW_WIDTH,
                height: OFFER_WINDOW_HEIGHT
            );
            DrawTheirOffer(theirOfferRect);
            
            // Arrows
            Rect tradeArrowsRect = new Rect(
                x: ourOfferRect.xMax + DEFAULT_SPACING,
                y: container.yMin,
                width: TRADE_ARROWS_WIDTH,
                height: TRADE_ARROWS_HEIGHT
            );
            Texture arrowsTexture = ContentFinder<Texture2D>.Get("tradeArrows");
            Widgets.DrawTextureFitted(tradeArrowsRect, arrowsTexture, 1f);
            
            // Update button
            Rect updateButtonRect = new Rect(
                x: tradeArrowsRect.xMin,
                y: tradeArrowsRect.yMax + DEFAULT_SPACING,
                width: TRADE_BUTTON_WIDTH,
                height: TRADE_BUTTON_HEIGHT
            );
            if (Widgets.ButtonText(updateButtonRect, "Phinix_trade_updateButton".Translate()))
            {
                // TODO: Update trade
            }
            
            // Reset button
            Rect resetButtonRect = new Rect(
                x: tradeArrowsRect.xMin,
                y: updateButtonRect.yMax + DEFAULT_SPACING,
                width: TRADE_BUTTON_WIDTH,
                height: TRADE_BUTTON_HEIGHT
            );
            if (Widgets.ButtonText(resetButtonRect, "Phinix_trade_resetButton".Translate()))
            {
                // TODO: Reset trade
            }
            
            // Cancel button
            Rect cancelButtonRect = new Rect(
                x: tradeArrowsRect.xMin,
                y: resetButtonRect.yMax + DEFAULT_SPACING,
                width: TRADE_BUTTON_WIDTH,
                height: TRADE_BUTTON_HEIGHT
            );
            if (Widgets.ButtonText(cancelButtonRect, "Phinix_trade_cancelButton".Translate()))
            {
                Instance.CancelTrade(tradeId);
            }
            
            // Our confirmation
            // TODO: Ellipsise display name length if it's going to spill over
            Rect ourConfirmationRect = new Rect(
                x: ourOfferRect.xMin,
                y: ourOfferRect.yMax + DEFAULT_SPACING,
                width: OFFER_WINDOW_WIDTH,
                height: OFFER_CONFIRMATION_HEIGHT
            );
            Widgets.CheckboxLabeled(
                rect: ourConfirmationRect,
                label: ("Phinix_trade_confirmOurTradeCheckbox" + (tradeAccepted ? "Checked" : "Unchecked")).Translate(), // Janky-looking easter egg, just for you
                checkOn: ref tradeAccepted
            );
            
            // Their confirmation
            // TODO: Ellipsise display name length if it's going to spill over
            Rect theirConfirmationRect = new Rect(
                x: theirOfferRect.xMin,
                y: theirOfferRect.yMax + DEFAULT_SPACING,
                width: OFFER_WINDOW_WIDTH,
                height: OFFER_CONFIRMATION_HEIGHT
            );
            Instance.TryGetOtherPartyAccepted(tradeId, out bool otherPartyAccepted);
            Widgets.CheckboxLabeled(
                rect: theirConfirmationRect,
                label: ("Phinix_trade_confirmTheirTradeCheckbox" + (otherPartyAccepted ? "Checked" : "Unchecked")).Translate(TextHelper.StripRichText(GetOtherPartyDisplayName())), // Jankier-looking easter egg, just for you
                checkOn: ref otherPartyAccepted
            );
        }

        /// <summary>
        /// Draws our offer within the given container.
        /// </summary>
        /// <param name="container">Container to draw within</param>
        private void DrawOurOffer(Rect container)
        {
            // Title
            Rect titleRect = new Rect(
                x: container.xMin,
                y: container.yMin,
                width: container.width,
                height: OFFER_WINDOW_TITLE_HEIGHT
            );
            
            // Set the text style
            Text.Anchor = TextAnchor.MiddleCenter;
            
            // Draw the title
            Widgets.Label(titleRect, "Our Offer");
            
            // Reset the text style
            Text.Anchor = TextAnchor.UpperLeft;
            
            // Draw a placeholder
            Widgets.DrawMenuSection(container.BottomPartPixels(container.height - titleRect.height));
        }

        /// <summary>
        /// Draws their offer within the given container.
        /// </summary>
        /// <param name="container">Container to draw within</param>
        private void DrawTheirOffer(Rect container)
        {
            // Title
            Rect titleRect = new Rect(
                x: container.xMin,
                y: container.yMin,
                width: container.width,
                height: OFFER_WINDOW_TITLE_HEIGHT
            );
            
            // Set the text style
            Text.Anchor = TextAnchor.MiddleCenter;
            
            // Draw the title
            Widgets.Label(titleRect, "Their Offer");
            
            // Reset the text style
            Text.Anchor = TextAnchor.UpperLeft;
            
            // Draw a placeholder
            Widgets.DrawMenuSection(container.BottomPartPixels(container.height - titleRect.height));
        }
    }
}