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

def rgb2gray(rgb):
    return np.dot(rgb[...,:3], [0.2989, 0.5870, 0.1140])

def process_data(path):

	#Cogemos todos los archivos jpg que encontremos dentro de la carpeta especificada
	output = []

	for item in os.listdir(path):

		if os.path.exists(path + item + "/Structural Similarity/"):
			shutil.rmtree(path + item + "/Structural Similarity/")  
		os.mkdir(path + item + "/Structural Similarity/")

		for root, dirs, files in os.walk(path + item + "/Images/"):
			itemOutput = []
			for file in files:
				if '.jpg' in file:
					original = cv2.imread(get_Original_Image(path, item))
					new = cv2.imread(path + item + "/Images/" + file)

					(score, diff) = ssim(original, new, full=True, multichannel=True)

					outText = file.replace('Screenshot_Shape_', '')
					outText = outText.replace('Size_', '')
					outText = outText.replace('.jpg', ':')

					#print(outText + " Structural Similarity: {} %".format(round(score * 100, 2)))

					diff = (diff * 255).astype('uint8') 
					cv2.imwrite(path + item + "/Structural Similarity/Diff_" + file, rgb2gray(diff))

					itemOutput.append(outText + str(round(score * 100, 2)))
			
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
		filewriter = csv.writer(csvfile, delimiter=',',
							quotechar='|', quoting=csv.QUOTE_MINIMAL)
		filewriter.writerow(["Forma", "Tamaño", "Acierto Medio"])
		for out in mediaResult:
			filewriter.writerow([out])

def save_item_to_csv(item, output):
	with open('Results' + item + '.csv', 'w', newline='') as csvfile:
		filewriter = csv.writer(csvfile, delimiter=',',
								quotechar='|', quoting=csv.QUOTE_MINIMAL)
		filewriter.writerow(["Forma", "Tamaño", "Acierto"])
		for out in output[len(output) - 1]:
			filewriter.writerow([out])

def get_Original_Image(path, item):

	for root, dirs, files in os.walk(path + item):
		for file in files:
			if 'Original' in file:
				img = "/" + file
	
	return path + item + img

process_data("images/")