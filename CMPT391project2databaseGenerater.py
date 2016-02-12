#saleId, transactionId, datetime, storename, city, province/state, region, country, item, price, catagory, department
#country->region->province/state->city->storename, Department->catagory->item,price

import random

def main():
	month = ["Jan ", "Feb ", "Mar ", "Apr ", "May ", "June ", "July ", "Aug ", "Sept ", "Oct ", "Nov ", "Dec "]
	year = 2000
	
	store = ["edmonton1", "edmonton2", "edmonton3", "edmonton4", "phoneix1", "phoenix1", "phoenix2", "phoenix3"]
	storeCity = {"edmonton1":"edmonton", "edmonton2":"edmonton", "edmonton3":"edmonton", "edmonton4":"edmonton", "phoneix1":"phoenix", "phoenix1":"phoenix", "phoenix2":"phoenix", "phoenix3":"phoenix"}
	cityProvince = {"edmonton":"alberta","phoenix":"arizona"}
	provinceRegion = {"alberta":"west", "arizona":"west"}
	provinceCountry = {"alberta":"canada", "arizona":"usa"}
	
	item = ["item1", "item2", "item3", "item4"]
	itemPrice = {"item1":"price1","item2":"price2","item3":"price3","item4":"price4"}
	itemCatagory = {"item1":"catagory1", "item2":"catagory1", "item3":"catagory2", "item4":"catagory3"}
	catagoryDepartment = {"catagory1":"depatment1", "catagory2":"department2", "catagory3":"department2"}
	monthNum = 0
	dayNum = 1
	saleID = 0
	while year <= 2015:
		for numTransactionsInDay in range(100, random.randint(100, 200)):
			i = random.randint(0, len(store)-1)
			for transactionID in range(1, random.randint(1,10)):
				j = random.randint(0, len(item)-1)
				print(str(saleID) +", "+ str(transactionID) +", ",end ="")
				print(month[monthNum] +" "+ str(dayNum) +" "+ str(year) +", ",end="")
				print(store[i] +", "+ storeCity[store[i]] +", "+ cityProvince[storeCity[store[i]]] +", "+ provinceRegion[cityProvince[storeCity[store[i]]]] +", "+ provinceCountry[cityProvince[storeCity[store[i]]]] +", ",end="")
				print(item[j] +", "+ itemPrice[item[j]] +", "+ itemCatagory[item[j]] +", "+ catagoryDepartment[itemCatagory[item[j]]])
			saleID+=1	
		if monthNum in [3,5,8,10] and dayNum == 30:
			monthNum+=1
			dayNum = 1
		elif monthNum in [0,2,4,6,7,9] and dayNum == 31:
			monthNum+=1
			dayNum = 1
		elif monthNum == 1 and dayNum == 28:
			monthNum+=1
			dayNum = 1
		elif monthNum == 11 and dayNum == 31:
			monthNum = 0
			daynum = 1
			year+=1
		else:
			dayNum+=1
main()