#saleId, transactionId, datetime, storename, city, province/state, region, country, item, price, catagory, department
#country->region->province/state->city->storename, Department->catagory->item,price

import random
import pymssql
import sys

#number of transactions per day
minTransPerDay = 50
maxTransPerDay = 100

numOfYears = 10

class dbConn:
        
        
        def __init__(self, server, dbName):
                try:
                        self.conn = pymssql.connect(host=server, database=dbName)
                        self.cursor = self.conn.cursor()
                        print("connected to db")
                except(e):
                        print("error connecting!")
                        print(e)


        def mkTable(self, tableName, columns):
                #drop the table first to clear it out if it alread exists
                self.cursor.execute("IF OBJECT_ID('%s', 'U') IS NOT NULL DROP TABLE %s;" % (tableName, tableName))
                self.conn.commit()
                #next, create the table
                self.cursor.execute("CREATE TABLE %s (id INT NOT NULL IDENTITY(1,1), PRIMARY KEY (id));" % (tableName))
                self.conn.commit()

                #add all the given columns
                for i in columns:
                        columnName = str(i["name"])
                        columnType = str(i["type"])
                        self.cursor.execute("ALTER TABLE dbo.stores ADD %s %s;" % (columnName, columnType))
                        self.conn.commit()

        def addStore(self, tableName, values):
                cmd = "INSERT INTO %s (sale_id, transaction_id, month, day, year, storename, city, province, region, country, item, price, catagory, department) VALUES(" % (tableName)

                count = 0
                for i in values:
                        if ( isinstance(i, int)):
                                cmd += "%d"
                        else:
                                cmd += "%s"
                        if ( count < len(values) - 1):
                                cmd += ", "
                        count+=1

                cmd += ");"
#                print(cmd)
                self.cursor.execute(cmd, values)
                self.conn.commit()
                
                
                

dbTable = [
        {"name": "sale_id", "type": "INTEGER"},
        {"name": "transaction_id", "type": "INTEGER"},
        {"name": "month", "type": "VARCHAR(100)"}, 
        {"name": "day", "type": "INTEGER"},
        {"name": "year", "type": "VARCHAR(100)"},
        {"name": "storename", "type": "VARCHAR(100)"},
        {"name": "city", "type": "VARCHAR(100)"},
        {"name": "province", "type": "VARCHAR(100)"},
        {"name": "region", "type": "VARCHAR(100)"},
        {"name": "country", "type": "VARCHAR(100)"},
        {"name": "item", "type": "VARCHAR(100)"},
        {"name": "price", "type": "INTEGER"},
        {"name": "catagory", "type": "VARCHAR(100)"},
        {"name": "department", "type": "VARCHAR(100)"}
]
                        
def main():

        db = dbConn("127.0.0.1", "CMPT491-Warehouse")
        db.mkTable("stores", dbTable)

        month = ["Jan ", "Feb ", "Mar ", "Apr ", "May ", "June ",\
                 "July ", "Aug ", "Sept ", "Oct ", "Nov ", "Dec "]
        year = 2000
        
        store = ["edmonton1", "edmonton2", "edmonton3", "edmonton4",\
                 "phoneix1", "phoenix2", "phoenix3", "phoenix4",\
                 "houston1", "houston2", \
                 "calgary1", "calgary2", "calgary3",\
                 "oxnard1", "oxnard2",\
                 "jersey city1", "jersey city2",\
                 "montreal1","montreal2",\
                 "vancouver1","vancouver2",\
                 "anchorage1"]
        storeCity = {"edmonton1":"edmonton", "edmonton2":"edmonton",\
                     "edmonton3":"edmonton", "edmonton4":"edmonton",\
                     "phoneix1":"phoenix", "phoenix2":"phoenix",\
                     "phoenix3":"phoenix", "phoenix4":"phoenix",\
                     "houston1":"houston", "houston2":"houston", \
                     "calgary1":"calgary","calgary2":"calgary",\
                     "calgary3":"calgary",\
                     "oxnard1":"oxnard", "oxnard2":"oxnard",\
                     "jersey city1":"jersey city", "jersey city2":"jersey city",\
                     "montreal1":"montreal","montreal2":"montreal",\
                     "vancouver1":"vancouver","vancouver2":"vancouver",\
                     "anchorage1":"anchorage"}
        cityProvince = {"edmonton":"alberta","phoenix":"arizona",\
                        "houston":"texas", "calgary":"alberta",\
                        "oxnard":"california", "jersey city":"new jersey",\
                        "montreal":"quebec","vancouver":"BC",\
                        "anchorage":"alaska"}
        provinceRegion = {"alberta":"west", "arizona":"west", "texas":"south",\
                          "new jersey":"east", "california":"west",\
                          "quebec":"east", "BC":"west", "alaska":"north"}
        provinceCountry = {"alberta":"canada", "arizona":"usa", "texas":"usa",\
                           "new jersey":"usa", "california":"usa",\
                           "quebec":"canada", "BC":"canada",\
                           "alaska":"usa"}
        
        item = ["mud", "street", "sledge", "claw", "finishing", "one inch",\
                "oak", "base", "basket", "foot"]
        itemPrice = {"mud":"150","street":"100","sledge":"70","claw":"20",\
                     "finishing":"5", "one inch":"10", "oak":"25", "base":"5",\
                     "basket":"15", "foot":"15"}
        itemCatagory = {"mud":"tire", "street":"tire", "sledge":"hammer",\
                        "claw":"hammer", "finishing":"nail","one inch":"nail",\
                        "oak":"bat", "base":"ball",\
                        "basket":"ball","foot":"ball"}
        catagoryDepartment = {"tire":"automotive", "hammer":"hardware",\
                              "nail":"hardware", "bat":"sporting",\
                              "ball":"sporting"}
        monthNum = 0
        dayNum = 1
        saleID = 0


        targetYear = year + numOfYears;
        while year <= targetYear:
                for numTransactionsInDay in range(minTransPerDay, random.randint(minTransPerDay, maxTransPerDay)):
                        i = random.randint(0, len(store)-1)
                        for transactionID in range(1, random.randint(1,10)):
                                j = random.randint(0, len(item)-1)


                                db.addStore("dbo.stores", (int(saleID), int(transactionID), str(month[monthNum]), int(dayNum), str(year), str(store[i]), str(storeCity[store[i]]), str(cityProvince[storeCity[store[i]]]), str(provinceRegion[cityProvince[storeCity[store[i]]]]), str(provinceCountry[cityProvince[storeCity[store[i]]]]), str(item[j]), int(itemPrice[item[j]]), str(itemCatagory[item[j]]), str(catagoryDepartment[itemCatagory[item[j]]]) ) )


                                
#                                print(str(saleID) +", "+ str(transactionID)\
#                                      +", ",end ="")
#                                print(month[monthNum] +" "+ str(dayNum)\
#                                      +" "+ str(year) +", ",end="")
#                                print(store[i] +", "+ storeCity[store[i]] \
#                                      +", "+ cityProvince[storeCity[store[i]]]\
#                                      +", "+ provinceRegion[cityProvince[storeCity[store[i]]]] \
#                                      +", "+ provinceCountry[cityProvince[storeCity[store[i]]]]\
#                                      +", ",end="")
#                                print(item[j] +", "+ itemPrice[item[j]] +", "+ \
#                                      itemCatagory[item[j]] +", "+ \
#                                      catagoryDepartment[itemCatagory[item[j]]])
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
