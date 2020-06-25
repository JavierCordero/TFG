from skimage.metrics import structural_similarity as ssim
import matplotlib.pyplot as plt
import numpy as np
import argparse
import imutils
import shutil
from PIL import Image
import cv2
import os
import csv
import re

def process_data(path):

	#Cogemos todos los archivos jpg que encontremos dentro de la carpeta especificada
	output = []

	for item in os.listdir(path):

		if os.path.exists(path + item + "/Structural Similarity/"):
			shutil.rmtree(path + item + "/Structural Similarity/")  
		os.mkdir(path + item + "/Structural Similarity/")

		black = Image.open(get_NOBG_Image(path, item)) # Can be many different formats.
		blackwidth, blackheight = black.size  # Get the width and hight of the image for iterating over
		blackheightpixel_values = list(black.getdata())

		for root, dirs, files in os.walk(path + item + "/Images/"):
			itemOutput = []
			for file in files:
				if '.jpg' in file:
				
					original = Image.open(get_Original_Image(path, item))
					originalheightpixel_values = list(original.getdata())

					new = Image.open(path + item + "/Images/" + file)
					newheightpixel_values = list(new.getdata())

					fallo = 0
					acierto = 0

					for w in range(blackwidth):
						for h in range(blackheight):
							if blackheightpixel_values[blackwidth*h+w][0] != 0 and blackheightpixel_values[blackwidth*h+w][1] != 0 and blackheightpixel_values[blackwidth*h+w][2] != 0:
								if(originalheightpixel_values[blackwidth*h+w][0] != newheightpixel_values[blackwidth*h+w][0] and 
								originalheightpixel_values[blackwidth*h+w][1] != newheightpixel_values[blackwidth*h+w][1] and
								originalheightpixel_values[blackwidth*h+w][2] != newheightpixel_values[blackwidth*h+w][2]):
									fallo += 1
								else:
									acierto += 1

					outText = file.replace('Screenshot_Shape_', '')
					outText = outText.replace('Size_', '')
					outText = outText.replace('.jpg', ':')

					itemOutput.append(outText + str(round((acierto * 100) / (acierto + fallo), 2)))

					print(outText  + str(round((acierto * 100) / (acierto + fallo), 2)))

			output.append(itemOutput)

		#Guardamos cada uno de los items de manera independiente en un csv distinto para tener un backup de los datos
		save_item_to_csv(item, output)

	#Ahora pasamos a comparar todos los datos para sacar la media de cada figura
	data = np.asarray(output)

	a, b = data.shape
	result = np.zeros(b)
	for p in data:
		i = 0
		for r in p:
			r = np.array2string(r)
			result[i] += float(r[np.char.find(r, ':') + 1:-1])
			i += 1
	
	i = 0

	mediaResult = []

	for k in result:
		k = k / a
		form = data[0][i][0:np.char.find(data[0][i], '_')]
		size = data[0][i][np.char.find(data[0][i], '_') + 1:np.char.find(data[0][i], ':')]
		mediaResult.append(form + "_" + size + ":" + str(k))
		i+= 1

	save_final_result_to_csv(mediaResult)
		
def save_final_result_to_csv(mediaResult):
	with open('Final_Results' + '.csv', 'w', newline='') as csvfile:
		filewriter = csv.writer(csvfile, delimiter=',')
		filewriter.writerow(["Forma" "Tamaño" "Acierto Medio"])
		for out in mediaResult:
			filewriter.writerow([out])

def save_item_to_csv(item, output):
	with open('Results' + item + '.csv', 'w', newline='') as csvfile:
		filewriter = csv.writer(csvfile, delimiter=',')
		filewriter.writerow(["Forma" "Tamaño" "Acierto"])
		for out in output[len(output) - 1]:
			filewriter.writerow([out])

def get_Original_Image(path, item):
	for root, dirs, files in os.walk(path + item):
		for file in files:
			if 'Desired' in file:
				img = "/" + file
	
	return path + item + img

def get_NOBG_Image(path, item):
	for root, dirs, files in os.walk(path + item):
		for file in files:
			if 'NoBG' in file:
				img = "/" + file
	
	return path + item + img

process_data("images/")