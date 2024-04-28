using Azure.Storage.Blobs;
using File = System.IO.File;

public class AzureBlobStorageAssessment_SLD_FA2
{
    //Connection string that connects you to the azure storage resource
    static string connectionString = "";
    //Name of the storage resource container
    static string containerName = "";
    
    public static void Main(string[] args)
    {
        //Folder path where the program gets the files that you want to upload
        string myfolderPath;

        //Prompts user to enter the folder path 
        Console.WriteLine("\nEnter a folder path where files are stored that you might want to upload or list: ");
        string newfolderPath = Console.ReadLine();
        //assighns user input to the folderpath string
        myfolderPath = newfolderPath;

        while (true) { 
            //Displays options for the user to choose from
            Console.WriteLine("\nAzure blob storage container:");
            Console.WriteLine("\n1.Upload");
            Console.WriteLine("2.List");
            Console.WriteLine("3.Download");
            Console.WriteLine("4.Delete");

            //Prompts user to select an option
            Console.Write("\nSelect a number or enter the action: ");
            string optionprompt = Console.ReadLine();

            //if user inputs is to upload to the blob storage then the code handels the task
            if (optionprompt == "1" || optionprompt == "Upload") 
            {
                try
                {
                    //gets files  from the directory given with the folder path provided
                    var files = Directory.GetFiles(myfolderPath, "*", SearchOption.AllDirectories);
                    BlobContainerClient containerC = new BlobContainerClient(connectionString, containerName);

                    //for each file in the files the file name gets replaced with the folder path file directory
                    foreach (var file in files)
                    {
                        var filePathOverBlobCloud = file.Replace(myfolderPath, string.Empty);
                        using (MemoryStream mystream = new MemoryStream(File.ReadAllBytes(file)))
                        {
                            //uploads file to the blob and displays a successful upload in the comand line
                            containerC.UploadBlob(filePathOverBlobCloud, mystream);
                            Console.WriteLine("\nBlob: " + filePathOverBlobCloud + " Was successfully Uploded!");
                        }
                    }
                    //runs the code section and breaks the while loop when done
                    Console.ReadLine();
                    continue;
                }
                catch (Exception e)
                {
                    //Displays when an exception error occurs
                    Console.WriteLine("\nMake Sure folder path was correct!!");
                    //Breaks while loop when done
                    continue;
                }
                
            }

            //if user inputs is to list all the blobs then the code handels the task
            else if (optionprompt == "2" || optionprompt == "List")
            {
                try
                {
                    BlobServiceClient myblobServiceClient = new BlobServiceClient(connectionString);
                    BlobContainerClient mycontainerClient = myblobServiceClient.GetBlobContainerClient(containerName);

                    //variable blobs are equal to all blobs in the container client
                    var blobs = mycontainerClient.GetBlobs();
                    //For each blob found in the container gets displayed in the command line 
                    foreach (var blob in blobs)
                    {
                        Console.WriteLine(blob.Name);
                        BlobClient blobCLient = mycontainerClient.GetBlobClient(blob.Name);
                        var files = Directory.GetFiles(myfolderPath, "*", SearchOption.AllDirectories);
                    }
                    //runs the code section and breaks the while loop when done
                    Console.ReadLine();
                    continue;
                }
                catch (Exception e)
                {
                    //Displays when an exception error occurs
                    Console.WriteLine("\nError could not find blobs in container!!");
                    //Breaks while loop when done
                    continue;
                }
                
            }

            //if user inputs is to Download the blobs in the container then the code handels the task
            else if (optionprompt == "3" || optionprompt == "Download")
            {
                //Try catch to handel exception errors
                try
                {
                    BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                    //variable blobs are equal to all blobs in the container client
                    var blobs = containerClient.GetBlobs();
                    //For each blob found in the container gets downloaded and displays that it is downloading in the command line
                    foreach (var blob in blobs)
                    {
                        Console.WriteLine("\nDownloading... " + blob.Name);
                        BlobClient blobCLient = containerClient.GetBlobClient(blob.Name);
                        blobCLient.DownloadTo(@"D:\CTU 2024 semester 1\Subjects\SLD521\FA2\Root\Downloaded\" + blob.Name);
                    }
                    //runs the code section and breaks the while loop when done
                    Console.ReadLine();
                    continue;
                }
                catch (Exception e)
                {
                    //Displays when an exception error occurs
                    Console.WriteLine("\nError could not find blobs in container to download!!");
                    //Breaks while loop when done
                    continue;
                }
            }

            //if user inputs is to Delete a blob in the container then the code handels the task
            else if (optionprompt == "4" || optionprompt == "Delete")
            {
                try
                {
                    // string blobName is the variable name for the blob that the user wants to delete
                    string blobName;

                    //prompts user to the name of the blob they wish to delete
                    Console.Write("\nEnter the name of the blob you wish to delete: ");
                    string Deleteblob = Console.ReadLine();
                    //assighns user input to the blobName string
                    blobName = Deleteblob;

                    //Deletes the blob
                    BlobClient blobClient = new BlobClient(connectionString, containerName, blobName);
                    blobClient.Delete();
                    //Displays that the blob was successfully deleted and breaks the while loop
                    Console.WriteLine("Blob: " + blobName + " was Deleted!");
                    Console.ReadLine();
                    continue;
                }
                catch (Exception e)
                {
                    //Displays when an exception error occurs
                    Console.WriteLine("\nCould not find blob! Make sure blob name is correct.");
                    //Breaks while loop when done
                    continue;
                }
    
            }
        }
    }
}