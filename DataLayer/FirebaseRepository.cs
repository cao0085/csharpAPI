using Google.Cloud.Firestore;
using RestApiPractice.Providers;

public abstract class FirebaseRepository
{
    protected readonly FirestoreDb _firestoreDb;

    protected FirebaseRepository(IFirestoreProvider firestoreProvider)
    {
        _firestoreDb = firestoreProvider.GetFirestoreDb();
    }
}
