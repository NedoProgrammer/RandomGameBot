import sys
import os
import random
from google_images_download import google_images_download

'''
[1] - current directory
[2] - game hash
[3] - query
'''

directory = sys.argv[1]
game_hash = sys.argv[2]
query = sys.argv[3]
output = os.path.join(directory, "Games", "GoogleForMe", game_hash)

response = google_images_download.googleimagesdownload()


def download_with_format(image_format: str):
    response.download(
        {"keywords": query, "limit": 10, "format": image_format, "output_directory": output, "print_urls": False,
         "silent_mode": True, "no_numbering": True, "no_directory": True})


download_with_format("png")
download_with_format("jpg")
download_with_format("gif")

files = os.listdir(output)
if "Data.json" in files:
    files.remove("Data.json")

index = random.randint(0, len(files) - 1)
leave = files[index]
for file in files:
    if file is not leave:
        os.remove(os.path.join(output, file))

os.rename(os.path.join(output, leave), os.path.join(output, "image" + os.path.splitext(leave)[1]))