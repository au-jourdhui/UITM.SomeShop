use SomeShopDatabase;
go

insert Brands(Name, Country, Logo, BackgroundImage) values
  (N'TianDe', N'Russia', N'tiande-logo.png', N'tiande-bg.png')      -- 1
, (N'Amrita', N'India', N'amrita-logo.png', N'amrita-bg.png')       -- 2
, (N'Unice', N'Europe', N'unice-logo.png', N'unice-bg.png')        -- 3
, (N'Flormar', N'Turkey', N'flomar-logo.png', N'flomar-bg.png')      -- 4
, (N'Biolika', N'Ukraine', N'biolika-logo.png', N'biolika-bg.png')  -- 5
, (N'Thalia', N'Japan', N'thalia-logo.png', N'thalia-bg.png')      -- 6

insert Products(Name, BrandId, Description) values
/*1*/  (N'Fucus extract', 5, N'Release form: liquid extract. Functional food product. Fucus extract has anti-inflammatory, wound-healing, regenerating, antibacterial, analgesic, sedative, sedative, immunostimulating, diuretic, choleretic, detoxifying, anti-sclerotic, liquid, anticoagulant action: propylene glycol base. Fucus extract contains a full set of micro- and macroelements (iron, iodine, potassium, calcium, sodium, magnesium, silicon, selenium, phosphorus, zinc, sulfur, etc.), almost all vitamins necessary for a person, a balanced set of organic acids and unique polysaccharides (alginates, laminaran, fucoidan), which are practically absent in terrestrial plants. An important fact is that all of these substances are contained in fucus in organic form. The extract helps to reduce blood cholesterol levels. Large amounts of iodine and triiodotyrosine gently stimulate thyroid function. Fucus alginates remove radionuclides and heavy metal salts from the body, regulate water-salt metabolism, normalize digestion, and cleanse the intestines. Also, alginates stimulate the healing processes of wounds and burns, prevent the development of infection in damaged tissues. Laminarin fucus strengthens the walls of blood vessels, normalizes blood supply to the heart and brain, prevents the formation of blood clots and malignant tumors, increases immunity, has a calming effect on the nervous system, and promotes sound sleep. Fucoidan has antiviral, immunoregulatory, antitumor (it stops the growth and development of new blood vessels that feed the tumor) properties. Fucus iodine normalizes the metabolism in the subcutaneous fatty tissue, activates the breakdown of fats and increases lipolysis, thus contributes to the disappearance of obesity - cellulite. Fucus normalizes the synthesis of sex hormones, stimulates the full functioning of the thyroid gland, replenishes the needs of the human body for essential amino acids, vitamins, polyunsaturated fatty acids type "Omega". The extract helps to strengthen the cardiovascular system, makes the walls of blood vessels more elastic and durable, is a powerful detoxifier and is indispensable for removing toxins from the body. Fucus is very effective for weight loss, its components stimulate metabolism, hinder the absorption of carbohydrates, and lower blood sugar levels. The components of fucus have a beneficial effect on the hormonal background of women during menopause and also normalize the menstrual cycle, strengthen the immune system, improve the condition of the skin and hair. Useful substances that fucus is rich in act in a complex on the body and have an effect on almost all organs and systems. Therefore, fucus extract is used in the treatment of diseases of the genitourinary, respiratory and nervous systems, gastrointestinal tract, diabetes mellitus, thyroid gland, asthma, allergies, skin diseases, muscle pain and inflammation of the joints, vitamin deficiency, dysbiosis, edema.')
/*2*/, (N'Flormar Pretty BB Cream SPF15', 4, N'With its light texture, this Beauty Balm provides an even complexion and a natural glow. Saturated with moisturizing and masking components, BB cream is ideally combined with the skin and helps to even out the tone. The unique formula, enriched with natural ingredients, provides the skin with a luxurious glow. The soft, creamy texture helps to maintain an even, flawless complexion throughout the day. Suitable for all skin types and also protects the skin from UV radiation with SPF 15.')
/*3*/, (N'Flormar Pretty Volumizing Mascara', 4, N'With just one sweep of the brush of the new Pretty Volume Mascara, lashes become twice as thick, and in half the time. Thanks to the special thickening and volumizing ingredients combined with moisturizing agents, the eyelashes look naturally thick, and the mascara''s polymers keep the curled shape of the eyelashes in place for a long time. Designed for this mascara, a long, stiff bristled brush separates the lashes and paints each lash individually.')
/*4*/, (N'Argan oil face and body cream', 6, N'Thalia Argan Oil Face & Body Cream stimulates the skin''s natural regeneration processes throughout the day. Organic argan oil, shea butter, provitamin B5 and vitamin E in the cream provide nutrition, hydration and the necessary care. Give your skin radiance, recovery and firmness. Suitable for daily use and skin care. Does not contain paraffin oils and parabens. Method of application: Apply to face and body with light massage movements. Precautions: For external use only. Avoid contact with eyes. In case of contact with eyes, rinse with plenty of water. Keep at room temperature, out of reach of children. Ingredients: Aqua, Glycerin, Ethylhexyl Stearate, Stearyl Alcohol, Butyrospermum Parkii Butter, Argania Spinosa Kernel Oil, Cyclopentasiloxane, Ethylhexyl Palmitate, Cyclohexasiloxane, Parfum, Ceteryl-Stetharate 12 , Cetearyl Alcohol, Cetyl Palmitate, Tocopheryl Acetate, Dimethicone, Panthenol, Sodium Polyacrylate, Allantoin, Tetrasodium EDTA, Phenethyl Alcohol, Caprylyl Alcohol, Citric Acid, Butylphenyl Methylpropional, Benzyl Benzoate, Alpha-Isomethylinn Alcohol, Citral, Coumarin, Linalool.')
/*5*/, (N'Spring, herbal tea', 5, N'Herbal tea "Spring" Phytocompositions of seasonal herbal teas of the "Native Nature" series allow you to harmonize human biorhythms and reduce the stress of the body when the seasons change. In summer, you can not only quench your thirst and cool the body, but also reduce the risk of cardiovascular diseases, allergies, and gastrointestinal disorders. In winter, the use of herbal tea will allow you to avoid colds and infectious diseases, to replenish your daily diet with vitamins. In spring and autumn, herbal tea will relieve you of exacerbations of chronic diseases, fill the body with vital energy. Phytoteas of the Rodnaya Priroda series are combined with other health products of the Biolika company, which increases their effectiveness and sustainability of the results obtained. Ingredients: licorice root, elderberry flowers, ginkgo - biloba, oregano herb, rose hips, linden flowers, raspberry leaves, cherry leaves, blackberry leaves. The balanced formula of herbal tea "Spring" was created to prevent seasonal allergies (hay fever) and acute respiratory diseases, as well as to prevent seasonal exacerbations of gastrointestinal diseases Allergy manifestations depend on both the hypersensitivity of the immune system and the ability of the liver to bind and excrete allergens. The liver is a "laboratory" that purifies the blood from all harmful substances. With the weakening of the liver function to neutralize toxic substances entering the blood, the body "starts" allergic reactions. The components of herbal tea are selected in such a way that they provide a hepatoprotective (restoration of liver cells) effect, choleretic effect, prevention of spring exacerbation of chronic diseases of the gastrointestinal tract (peptic ulcer, etc.). . "Spring" phytotea strengthens the walls of blood vessels, reduces the risk of cerebral circulation disorders, normalizes metabolic processes, is a source of vitamins and microelements, has a wonderful aroma of wild herbs.')
/*6*/, (N'Amrita Revitalizing Facial Toner, 200 ml', 2, N'The powerful Fitomelanin® ++ in combination with Essential extracts and herbal active ingredients promotes cell regeneration and prevents the signs of aging.')
/*7*/, (N'Lip balm Unice Divine', 3, N'UNICE balms will not only please with pleasant fruit aromas, but also will take care of beauty and nutrition of lips. Don''t forget to put in your handbag and enjoy the summer and the sun! How to use: Apply as needed on the lips as a stand-alone tool or before using lipstick / lip gloss. Precautions: For external use only. Store at room temperature. Avoid contact with eyes. In case of contact with eyes, rinse immediately with plenty of water. Keep out of reach of children. Avoid direct sunlight.')
/*8*/, (N'Beauty Balance, 30 caps.', 2, N'Beauty Balance is a special formula of vitamins, minerals and plant extracts for the health and beauty of skin, nails and hair, as well as energy and good mood for every day.')
/*9*/, (N'Unice Divine Moisturizing Lipstick, 4,2g', 3, N'Try the perfect Sensual Lips palette for daytime makeup! The very soft texture with shimmery particles will give the lips a seductive volume, and the castor oil in the lipstick will provide the necessary hydration. Imagine - you no longer need lip balm!')
/*10*/, (N'Sun Olives Body Milk Cream, Series "Hainan Tao"', 1, N'Anti-stress series "Hainan Tao" is created on the basis of natural ingredients. Perfect for daily use for all skin types and at all ages. With Sun Olives Body Milk, your skin will be soft and delicate like a child''s. A luxurious nourishing treatment for your velvety skin. The composition includes olive extract, allantoin, vitamin C, hyaluronic acid, thanks to which your skin breathes health and beauty!')
/*11*/, (N'Mineral powder UNICE Divine MCP01', 3, N'Multifunctional mineral powder UNICE Divine has unsurpassed long-lasting properties. High quality ingredients provide a natural look and a matte finish. The minerals in the powder maintain the proper level of hydration, creating a protective barrier between the skin''s surface and harmful external factors. Powder hides skin imperfections and prevents oily sheen. Ideal for maintaining and renewing your makeup throughout the day. Paraben and fragrance free.')
/*12*/, (N'Anti-inflammatory lotion, anti-acne series"Master Herb"', 1, N'Phyto-correction of problem skin with TianDe! Master Herb lotion is applied after washing with gel. Penetrating deep into the pores, it helps to remove all excess from the skin, thoroughly cleansing them. Has disinfecting properties, as it is designed to fight acne. Anti-inflammatory lotion, anti-acne series "Master Herb" - tones, refreshes, has an astringent effect; not only removes excess sebum, but also normalizes the activity of the sebaceous glands, restoring the pH of the skin. Plant extracts soothe, helping to reduce swelling and redness of the skin.')
/*13*/, (N'Anti-hair loss shampoo (anti-hair loss), series "Master Herb"', 1, N'Anti-baldness shampoo (anti-hair loss), "Master Herb" series - effectively stops and prevents premature hair loss. Strengthens blood supply in the area of the baldness focus. Activates metabolic processes. Restores the vitamin and mineral balance required for hair growth. Strengthens the roots, recreates the damaged hair cuticle (protective layer of the hair). Stimulates new hair growth. Recommended for thinning hair.')
/*14*/, (N'Chantal Cream-soufflé for hands and body "Willow and olive", 200 g', 2, N'Light hand and body cream provides intense hydration and nourishment throughout the day. Easily eliminates irritation and dryness, as well as accelerates the recovery process in the skin. The cream perfectly combines and provides comprehensive care for the skin of hands and body.')

insert into ProductImages(ProductId, Image) values
  (1,N'ekstrakt-fukusa-30-ml.jpg')
, (2,N'Flormar_pretty_BB_Cream_SPF15.jpg')
, (3,N'Flormar_Pretty_Volumizing_mascara.jpg')
, (4,N'thalia_cream.png')
, (5,N'spring-herbal-tea.jpg')
, (6,N'Amrita_Restoral_tonic_for_face.jpg')
, (7,N'Unice_lip_balm.png')
, (8,N'Beauty_Balance_30_caps.png')
, (9,N'Lipstick_For_Lips_Unice.png')
, (10,N'Cream_milk_for_body_Sun_olives.jpg')
, (11,N'Mineral_powder_unice.png')
, (12,N'Anti-inflammatory_lotion_series_antiacne.jpg')
, (13,N'Anti-baldness-shampoo.png')
, (14,N'Chantal_Cream-souffle_for_hands_and_body.png')

insert into Items(ProductId, Price, OldPrice) values
  (1, 8, null)
, (2, 7, null)
, (2, 6.5, null)
, (3, 6, null)
, (4, 22, null)
, (5, 4, 50)
, (6, 12, null)
, (7, 4, null)
, (8, 35.5, 420)
, (9, 10, null)
, (10,33, 389)
, (11, 20, null)
, (12, 10.3, 12.00)
, (13, 32, 38)
, (14, 5.5, null)

insert Categories(Name, ParentId) values
  (N'For face ', null) -- 1
, (N'Problem skin ', 1) -- 2
, (N'For young skin ', 1) -- 3
, (N'For mature skin ', 1) -- 4
, (N'For body ', null) -- 5
, (N'Anticellulite funds', 5) -- 6
, (N'Body Care ', 5) -- 7
, (N'Ortal ', null) -- 8
, (N'For hair ', null) -- 9
, (N'For children ', null) -- 10
, (N'Health ', null) -- 11
, (N'Women''s health ', 11) -- 12
, (N'Men''s health ', 13) -- 13
, (N'Power', null) -- 14
, (N'Oil ', 14) -- 15
, (N'Shroty ', 14) -- 16
, (N'Cocktails', 14) -- 17
, (N'Sesame and Bars', 14) -- 18
, (N'Decorative cosmetics', null) -- 19
, (N'Perfumery ', null) -- 20
, (N'Fitochai ', null) -- 21
, (N'Fitoproducts', null) -- 22
, (N'Detox organism ', null) -- 23
, (N'Antiarasitic phytopreparations', 23) -- 24
, (N'Vitamins and minerals', 23) -- 25
, (N'Support Immunity ', 23) -- 26
, (N'Healthy skin ', 23) -- 27
, (N'Respiratory system ', 23) -- 28
, (N'Cardiovascular system ', 23) -- 29
, (N'Musculoskeletal system', 23) -- 30
, (N'Nervous system ', 23) -- 31
, (N'Digestive system ', 23) -- 32
, (N'Kidneys and genitourinary system', 23) -- 33
, (N'Endocrine system ', 23) -- 34
, (N'Eye Health ', 23) -- 35
, (N'For home ', null) -- 36
, (N'Procedures', null) -- 37
, (N'Certificates', null) -- 38

insert CategoryProducts(ProductId, CategoryId) values
  (1,14)
, (1,22)
, (1,26)
, (2,2)
, (2,3)
, (3,1)
, (4,3)
, (4,4)
, (4,15)
, (5,21)
, (5,22)
, (6,2)
, (6,3)
, (6,4)
, (7,1)
, (8,25)
, (8,26)
, (9,1)
, (10,5)
, (11,1)
, (12,2)
, (12,27)
, (13,9)
, (14,5)

insert Characteristics(Name) values
  (N'Volume')            -- 1
, (N'Skin type')         -- 2
, (N'Color')             -- 3

insert OptionalParameters(Name) values
  (N'Volume')
, (N'Color')
, (N'Skin type')

insert OptionalParameterProducts(ItemId, OptionalParameterId, Value) values
  (2,2,N'BB Cream 01 Light')
, (3,2,N'BB Cream 02 Light Medium')

insert Roles(Name) values
  (N'User')
, (N'Administrator')

insert OrderDeliveryTypes(Type) values
  (N'DHL')
, (N'UPS')
, (N'Pickup')

insert OrderPaymentTypes(Type) values
  (N'Cash on Delivery')
, (N'Card Payment')

insert OrderStatuses(Status) values
  (N'In progress')
, (N'Ready for delivery')
, (N'Submitted')
, (N'Completed')
, (N'Cancelled')

insert ContactInfo(City, Address, Phones, TimeTable, GoogleUrl) values
  (N'London', N'Gresham House, Holborn, London, UK', N'+1234567891', N'<b>Mon - Sat:</b> 10:00 – 19:00;<b>Sun:</b> 10:00 - 17:00', N'https://www.google.com/maps/embed?pb=!1m14!1m8!1m3!1d2482.8454476810584!2d-0.1038316308237557!3d51.516051383493554!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x0!2zNTHCsDMxJzAwLjciTiAwwrAwNicxNS44Ilc!5e0!3m2!1sen!2sua!4v1619297964670!5m2!1sen!2sua')
, (N'Berlin', N'Neue Roßstraße 5', N'+1234567892', N'<b>Mon - Sat:</b> 10:00 – 18:30;<b>Sun:</b> day off', N'https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2428.218160936562!2d13.40768141534998!3d52.51139084462025!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x47a84e25df997fc5%3A0x4ccc7da9f6a8e140!2sNeue%20Ro%C3%9Fstra%C3%9Fe%205%2C%2010179%20Berlin%2C%20Germany!5e0!3m2!1sen!2sua!4v1619298018419!5m2!1sen!2sua')
, (N'Paris' , N'Grenelle 75015 Paris', N'+1234567893', N'<b>Mon - Sat:</b> 11:00 – 22:00;<b>Sun:</b> day off', N'https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d10501.605524110304!2d2.2839762339130107!3d48.85055603833441!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x47e6701bc3876107%3A0x95163ba7f7cbd88d!2sGrenelle%2C%2075015%20Paris%2C%20France!5e0!3m2!1sen!2sua!4v1619298068070!5m2!1sen!2sua')
