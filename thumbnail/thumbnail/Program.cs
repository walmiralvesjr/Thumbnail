using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string directoryPath = @"C:\Users\Junior\Videos"; // Mude para o seu diretório

        // Verifica se o diretório existe
        if (Directory.Exists(directoryPath))
        {
            // Obtém todos os arquivos de vídeo do diretório
            string[] videoFiles = Directory.GetFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly);

            foreach (string videoFile in videoFiles)
            {
                if (IsVideoFile(videoFile))
                {
                    string thumbnailPath = Path.ChangeExtension(videoFile, ".jpg");
                    GenerateThumbnail(videoFile, thumbnailPath);
                    Console.WriteLine($"Thumbnail gerada: {thumbnailPath}");
                }
            }
        }
        else
        {
            Console.WriteLine("Diretório não encontrado.");
        }
    }

    static bool IsVideoFile(string filePath)
    {
        string[] videoExtensions = { ".mp4", ".avi", ".mov", ".mkv", ".wmv" }; // Adicione mais extensões se necessário
        string extension = Path.GetExtension(filePath).ToLower();
        return Array.Exists(videoExtensions, ext => ext == extension);
    }

    static void GenerateThumbnail(string videoPath, string thumbnailPath)
    {
        string ffmpegPath = @"C:\ffmpeg\ffmpeg\bin\ffmpeg.exe"; // Mude para o seu caminho do FFmpeg
        string arguments = $"-i \"{videoPath}\" -ss 00:00:01.000 -vframes 1 \"{thumbnailPath}\""; // Captura uma imagem no 1 segundo

        ProcessStartInfo processInfo = new ProcessStartInfo
        {
            FileName = ffmpegPath,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = Process.Start(processInfo))
        {
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                string error = process.StandardError.ReadToEnd();
                Console.WriteLine($"Erro ao gerar thumbnail: {error}");
            }
        }
    }
}
