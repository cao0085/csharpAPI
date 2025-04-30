using Google.Cloud.Firestore;

namespace RestApiPractice.Providers.Firebase
{
    /// <summary>
    /// FirestoreProvider interface
    /// </summary>
    /// <remarks>
    /// This interface is used to provide FirestoreDb instance.
    /// </remarks>
    public interface IFirestoreProvider
    {
        FirestoreDb GetFirestoreDb();
    }
}

