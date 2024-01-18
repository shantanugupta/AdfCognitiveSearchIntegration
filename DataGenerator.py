from faker import Faker
import csv, json
import random
from datetime import datetime, timedelta

fake = Faker()

# Function to generate data for Categories table
def generate_categories_data():
    categories = [
        {'category_id': i + 1, 'category_name': fake.word()}
        for i in range(10000)  # Modify the range for the number of rows needed
    ]
    return categories

# Function to generate data for Products table
def generate_products_data():
    products = [
        {
            'product_id': i + 1,
            'product_name': fake.word(),
            'category_id': random.randint(1, 10000),  # Assuming 10000 categories
            'price': round(random.uniform(10, 1000), 2),
            'description': fake.sentence(),
            'image_url': fake.image_url(),
            'date_added': (datetime.now() - timedelta(days=random.randint(1, 365))).strftime('%Y-%m-%d')
        }
        for i in range(1000000)  # Modify the range for the number of rows needed
    ]
    return products

# Function to generate data for Orders table
def generate_orders_data():
    orders = [
        {'order_id': i + 1, 'order_date': (datetime.now() - timedelta(days=random.randint(1, 365))).strftime('%Y-%m-%d'), 'customer_name': fake.name()}
        for i in range(50000)  # Modify the range for the number of rows needed
    ]
    return orders

# Function to generate data for OrderProducts table
def generate_order_products_data():
    order_products = [
        {'order_id': random.randint(1, 50000), 'product_id': random.randint(1, 1000000)}  # Assuming 50000 orders and 1000000 products
        for _ in range(150000)  # Modify the range for the number of rows needed
    ]
    return order_products

# Write data to CSV files
def write_to_csv(data, filename):
    folderPath = "./generated_files/"
    with open(folderPath + filename, 'w', newline='', encoding='utf-8') as csvfile:
        writer = csv.DictWriter(csvfile, fieldnames=data[0].keys())
        writer.writeheader()
        writer.writerows(data)

# Write data to json files
def write_to_json(data, filename):
    folderPath = "./generated_files/"
    with open(folderPath + filename, 'w') as file:
        json.dump(data, file, indent=2)

# Generate data for each table
categories_data = generate_categories_data()
products_data = generate_products_data()
orders_data = generate_orders_data()
order_products_data = generate_order_products_data()


# Write data to CSV files
write_to_csv(categories_data, 'categories.csv')
write_to_csv(products_data, 'products.csv')
write_to_csv(orders_data, 'orders.csv')
write_to_csv(order_products_data, 'order_products.csv')

write_to_json(categories_data, 'categories.json')
write_to_json(products_data, 'products.json')
write_to_json(orders_data, 'orders.json')
write_to_json(order_products_data, 'order_products.json')