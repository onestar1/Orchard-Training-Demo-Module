using Lombiq.TrainingDemo.Models;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using System.Diagnostics.CodeAnalysis;

namespace Lombiq.TrainingDemo.Drivers;

// DisplayDrivers are for implementing the functionality for specific shapes generated by the DisplayManager. You can
// create a driver for any object (persisted or not-persisted) where you can implement their specific logic for
// generating reusable shapes. In case of an editor shape the model update and custom validation is also done in these
// drivers. Finally, you can create multiple drivers for one object and the DisplayManager will make sure that all of
// your drivers are used and their specific logic will be executed. Don't forget to register this class with the service
// provider (see: Startup.cs).
public sealed class BookDisplayDriver : DisplayDriver<Book>
{
    // So we have a Book object and we want to register some display shapes. For this you need to override the Display
    // or DisplayAsync methods depending on your code (only one can be used!). Ultimately, the DisplayManager will
    // return a shape that contains all (or some) of these shapes.
    [SuppressMessage(
        "StyleCop.CSharp.ReadabilityRules",
        "SA1114:Parameter list should follow declaration",
        Justification = "Necessary for comments.")]
    public override IDisplayResult Display(Book model, BuildDisplayContext context) =>
        // For the sake of demonstration we use Combined() here. It makes it possible to return multiple shapes from a
        // driver method - won't necessarily be displayed all at once!
        Combine(
            // Here we define a shape for the Title. It's not necessary to split these to atomic pieces but it would
            // make sense to make a reusable shape for the title, and it also makes overriding just these pieces
            // possible (like you hand this module over to somebody and they want to display the title differently on
            // their site, without modifying your module). There are multiple helpers you can use to render a shape
            // (display or edit) this View() comes handy when you don't want to bother with view models (the view model
            // will be a ShapeViewModel<Book> in this case, you'll see). In the Location helper you define a position
            // for the shape. "Header" means that it will be displayed in the Header zone. "1" means that it will be the
            // first in the Header zone. Soon you will see what the zones are.
            View($"{nameof(Book)}_Display_Title", model)
                .Location("Header: 1"),
            // Same applies here. This shape will be displayed in the Header zone too but in the second position.
            View($"{nameof(Book)}_Display_Author", model)
                .Location("Header: 2"),
            // Create a separate shape for the cover photo. This will go to a different zone, the "Cover" zone.
            View($"{nameof(Book)}_Display_Cover", model)
                .Location("Cover: 1"),
            // The shape for the description will be the first in the Content zone. However, you can see another
            // parameter here, that is the display type. It is used to differentiate circumstances of displaying a
            // shape. Let's say you want to display Title, Author and Cover all the time (no shape type parameter), but
            // the description will be displayed only if the display type is "Description". You'll see an example for
            // that. Though we can choose any display types the conventional ones are "Detail", "Summary" and
            // "SummaryAdmin" which are used for ContentItem-related shapes.
            View($"{nameof(Book)}_Display_Description", model)
                .Location("Description", "Content: 1"));

    // Now let's see what those zones are and slowly clarify all these things you've seen above!
    // NEXT STATION: Views/Book.cshtml.
}