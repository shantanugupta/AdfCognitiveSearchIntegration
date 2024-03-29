/****** Object:  StoredProcedure [dbo].[usp_LoadData]    Script Date: 1/18/2024 8:37:21 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[usp_LoadData]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[products]') AND type in (N'U'))
ALTER TABLE [dbo].[products] DROP CONSTRAINT IF EXISTS [FK__products__catego__5EBF139D]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[order_products_map]') AND type in (N'U'))
ALTER TABLE [dbo].[order_products_map] DROP CONSTRAINT IF EXISTS [FK__order_pro__produ__6477ECF3]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[order_products_map]') AND type in (N'U'))
ALTER TABLE [dbo].[order_products_map] DROP CONSTRAINT IF EXISTS [FK__order_pro__order__6383C8BA]
GO
/****** Object:  Index [ix_products_category_id]    Script Date: 1/18/2024 8:37:21 PM ******/
DROP INDEX IF EXISTS [ix_products_category_id] ON [dbo].[products]
GO
/****** Object:  Table [dbo].[stg_products]    Script Date: 1/18/2024 8:37:21 PM ******/
DROP TABLE IF EXISTS [dbo].[stg_products]
GO
/****** Object:  Table [dbo].[stg_orders]    Script Date: 1/18/2024 8:37:21 PM ******/
DROP TABLE IF EXISTS [dbo].[stg_orders]
GO
/****** Object:  Table [dbo].[stg_categories]    Script Date: 1/18/2024 8:37:21 PM ******/
DROP TABLE IF EXISTS [dbo].[stg_categories]
GO
/****** Object:  Table [dbo].[products]    Script Date: 1/18/2024 8:37:21 PM ******/
DROP TABLE IF EXISTS [dbo].[products]
GO
/****** Object:  Table [dbo].[orders]    Script Date: 1/18/2024 8:37:21 PM ******/
DROP TABLE IF EXISTS [dbo].[orders]
GO
/****** Object:  Table [dbo].[order_products_map]    Script Date: 1/18/2024 8:37:21 PM ******/
DROP TABLE IF EXISTS [dbo].[order_products_map]
GO
/****** Object:  Table [dbo].[categories]    Script Date: 1/18/2024 8:37:21 PM ******/
DROP TABLE IF EXISTS [dbo].[categories]
GO
/****** Object:  View [dbo].[vw_OrderDetails]    Script Date: 21-01-2024 21:06:38 ******/
DROP VIEW IF EXISTS [dbo].[vw_OrderDetails]
GO
/****** Object:  Table [dbo].[categories]    Script Date: 1/18/2024 8:37:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[categories](
	[category_id] [int] IDENTITY(1,1) NOT NULL,
	[category_name] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[category_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [uk_categories_category_name] UNIQUE NONCLUSTERED 
(
	[category_name] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[categories] ENABLE CHANGE_TRACKING WITH(TRACK_COLUMNS_UPDATED = ON)
GO
/****** Object:  Table [dbo].[order_products_map]    Script Date: 1/18/2024 8:37:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[order_products_map](
	[order_id] [int] NOT NULL,
	[product_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[order_id] ASC,
	[product_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[orders]    Script Date: 1/18/2024 8:37:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[orders](
	[order_id] [int] IDENTITY(1,1) NOT NULL,
	[order_date] [date] NULL,
	[customer_name] [varchar](100) NOT NULL,
	[order_number] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[order_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [uk_orders_order_number] UNIQUE NONCLUSTERED 
(
	[order_number] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[products]    Script Date: 1/18/2024 8:37:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[products](
	[product_id] [int] IDENTITY(1,1) NOT NULL,
	[product_name] [nvarchar](100) NULL,
	[category_id] [int] NULL,
	[price] [decimal](10, 2) NOT NULL,
	[description] [nvarchar](2000) NULL,
	[image_url] [varchar](1000) NULL,
	[date_added] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[product_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [uk_products_product_name] UNIQUE NONCLUSTERED 
(
	[product_name] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[products] ENABLE CHANGE_TRACKING WITH(TRACK_COLUMNS_UPDATED = ON)
GO
/****** Object:  Table [dbo].[stg_categories]    Script Date: 1/18/2024 8:37:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[stg_categories](
	[category_name] [varchar](100) NOT NULL,
	[category_id] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[stg_orders]    Script Date: 1/18/2024 8:37:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[stg_orders](
	[order_date] [date] NULL,
	[customer_name] [varchar](100) NOT NULL,
	[order_number] [int] NOT NULL,
	[product_name] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[stg_products]    Script Date: 1/18/2024 8:37:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[stg_products](
	[product_name] [nvarchar](max) NULL,
	[price] [decimal](10, 4) NULL,
	[description] [nvarchar](max) NULL,
	[image_url] [nvarchar](max) NULL,
	[date_added] [date] NULL,
	[category_name] [nvarchar](max) NULL,
	[product_id] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [ix_products_category_id]    Script Date: 1/18/2024 8:37:21 PM ******/
CREATE NONCLUSTERED INDEX [ix_products_category_id] ON [dbo].[products]
(
	[category_id] ASC
)
INCLUDE([product_name]) WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[order_products_map]  WITH CHECK ADD FOREIGN KEY([order_id])
REFERENCES [dbo].[orders] ([order_id])
GO
ALTER TABLE [dbo].[order_products_map]  WITH CHECK ADD FOREIGN KEY([product_id])
REFERENCES [dbo].[products] ([product_id])
GO
ALTER TABLE [dbo].[products]  WITH CHECK ADD FOREIGN KEY([category_id])
REFERENCES [dbo].[categories] ([category_id])
GO
/****** Object:  StoredProcedure [dbo].[usp_LoadData]    Script Date: 1/18/2024 8:37:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_LoadData]
	@batchSize INT = 1000,
	@delay VARCHAR(100) = '00:00:00:10'
AS
BEGIN
	SET NOCOUNT ON;

	-------------------------
	-- Sproc call 
	-- exec [dbo].[usp_LoadData] @batchSize =5000, @delay = '00:00:00:10'
	-------------------------

	---------------------------
	--------Loading categories
	---------------------------

	DECLARE @rowCount INT = 0,
			@msg VARCHAR(1000) = '';

	-- Add categories in one go. Assuming there won't be many categories
	INSERT INTO categories (category_name)
	SELECT DISTINCT t.category_name
	FROM stg_categories t
	LEFT JOIN categories c ON t.category_name = c.category_name
	WHERE c.category_id IS NULL;

	SELECT @rowCount = @@ROWCOUNT, @msg = CONCAT(@rowcount, ' rows inserted into categories');
	RAISERROR(@msg, 10, 1) WITH NOWAIT;

	TRUNCATE TABLE stg_categories;

	---------------------------
	--------Loading products
	---------------------------

	DECLARE @stg_products TABLE (
		product_name NVARCHAR(100),
		category_name NVARCHAR(100),
		price DECIMAL(10,2),
		description NVARCHAR(4000),
		image_url VARCHAR(100),
		date_added DATETIME,
		product_id INT
	);

	SET @rowCount = 1;

	WHILE (@rowCount > 0)
	BEGIN
		-- Delete top batchSize rows from stg_products and capture the deleted rows in @stg_products table variable
		DELETE TOP (@batchSize) t 
		OUTPUT 
			DELETED.product_name,
			DELETED.category_name,
			DELETED.price,
			DELETED.description,
			DELETED.image_url,
			DELETED.date_added,
			DELETED.product_id
		INTO @stg_products
		FROM stg_products t;

		-- Display the number of rows deleted from stg_products
		SELECT @rowCount = @@ROWCOUNT, @msg = CONCAT(@rowcount, ' rows deleted from stg_products');
		RAISERROR(@msg, 10, 1) WITH NOWAIT;

		-- Map categories for the selected batch from categories table and insert new products
		INSERT INTO products
		(product_name, category_id, price, description, image_url, date_added)
		SELECT t.product_name, c.category_id, t.price, SUBSTRING(t.description, 0, 4000), t.image_url, t.date_added 
		FROM @stg_products t
		INNER JOIN categories c ON t.category_name = c.category_name
		LEFT JOIN products p ON t.product_name = p.product_name
		WHERE p.product_id IS NULL;

		-- Display the number of rows inserted into products
		SELECT @rowCount = @@ROWCOUNT, @msg = CONCAT(@rowcount, ' rows inserted into products');
		RAISERROR(@msg, 10, 1) WITH NOWAIT;

		-- Get the number of records left in the staging table
		SELECT @rowCount = COUNT(*), @msg = CONCAT(@rowcount, ' rows left in stg_products') FROM stg_products;
		RAISERROR(@msg, 10, 1) WITH NOWAIT;
	END;

	---------------------------
	--------Loading orders
	---------------------------

	-- Add orders in batches
	DECLARE @stg_orders TABLE (
		order_number INT,
		customer_name VARCHAR(100),
		order_date DATETIME,
		product_name VARCHAR(100)
	);

	SET @rowCount = 1;

	WHILE (@rowCount > 0)
	BEGIN
		-- Delete top batchSize rows from stg_orders and capture the deleted rows in @stg_orders table variable
		DELETE TOP (@batchSize) t 
		OUTPUT DELETED.order_number, DELETED.customer_name, DELETED.order_date, DELETED.product_name
		INTO @stg_orders
		FROM stg_orders t;

		-- Display the number of rows deleted from stg_orders
		SELECT @rowCount = @@ROWCOUNT, @msg = CONCAT(@rowcount, ' rows deleted from stg_orders');
		RAISERROR(@msg, 10, 1) WITH NOWAIT;

		-- Insert into orders and capture inserted order details
		INSERT INTO orders (order_number, customer_name, order_date)
		SELECT t.order_number, MIN(t.customer_name) AS customer_name, MIN(t.order_date) AS order_date 
		FROM @stg_orders t
		LEFT JOIN orders o ON t.order_number = o.order_number
		WHERE o.order_id IS NULL
		GROUP BY t.order_number;

		-- Display the number of rows inserted into orders
		SELECT @rowCount = @@ROWCOUNT, @msg = CONCAT(@rowcount, ' rows inserted into orders');
		RAISERROR(@msg, 10, 1) WITH NOWAIT;

		-- Insert into order_products_map
		INSERT INTO order_products_map (order_id, product_id)
		SELECT o.order_id, p.product_id 
		FROM @stg_orders s
		INNER JOIN orders o ON s.order_number = o.order_number
		INNER JOIN products p ON p.product_name = s.product_name
		LEFT JOIN order_products_map op ON op.order_id = o.order_id AND op.product_id = p.product_id
		WHERE op.order_id IS NULL;

		-- Display the number of rows inserted into order_products_map
		SELECT @rowCount = @@ROWCOUNT, @msg = CONCAT(@rowcount, ' rows inserted into order_products_map');
		RAISERROR(@msg, 10, 1) WITH NOWAIT;

		-- Get the number of records left in the staging table
		SELECT @rowCount = COUNT(*), @msg = CONCAT(@rowcount, ' rows left in stg_orders') FROM stg_orders;
		RAISERROR(@msg, 10, 1) WITH NOWAIT;

		-- Introduce a delay
		WAITFOR DELAY @delay;
	END;
END;
GO
/****** Object:  View [dbo].[vw_OrderDetails]    Script Date: 21-01-2024 21:06:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_OrderDetails]
AS
SELECT o.order_date
	,o.order_number
	,o.customer_name
	,p.product_name
	,p.price
	,p.description
	,p.image_url
	,p.date_added
	,c.category_name
FROM orders o
INNER JOIN order_products_map op ON o.order_id = op.order_id
INNER JOIN products p ON p.product_id = p.product_id
INNER JOIN categories c ON p.category_id = c.category_id

GO
