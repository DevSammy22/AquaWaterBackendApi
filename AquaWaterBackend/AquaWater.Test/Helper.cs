using AquaWater.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Test
{
    public class Helper
    {
        public static List<User> GetUser()
        {
			var users = new List<User>()
			{
				new User() { Id = "c66b2e07-8329-4fb7-8070-b0ecf66ac21f",
							 FirstName = "Cristina",
							 LastName = "Morris",
							 CreatedAt = DateTime.Now,
							 UpdatedAt = DateTime.Now,
							  Email = "SingaporePalau@gmail.com",
							  UserName =  "SingaporePalau@gmail.com",
							  ProfilePictureUrl = "https://generated.photos/face-generator/626cf780c71546000ee8bc5a",
								  Location = {
										Country = "Singapore",
										State = "Palau",
										City = "Fruitdale",
										Street = "Stryker Street",
										HouseNumber = (224).ToString(),
										Latitude =(-62.588874).ToString(),
									    Longitude = (-133.503408).ToString()
					               }
				},


				new User() {Id = "64a93770-50a3-4f82-875f-50897b672e19",
							 FirstName = "Booth",
							 LastName = "Griffith",
							 CreatedAt = DateTime.Now,
							 UpdatedAt = DateTime.Now,
							 Email = "BoothGriffith@gmail.com",
							 UserName = "BoothGriffith@gmail.com",
							 ProfilePictureUrl = "https://generated.photos/face-generator/626cf780c71546000ee8bc5a",
								 Location = {
											 Country = "France, Metropolitan",
											 State = "Utah",
											 City = "Carlos",
										     Street = "Coleman Street",
										     HouseNumber = (524).ToString(),
										     Latitude = (36.405963).ToString(),
										     Longitude = (0.938056).ToString()
								  }

				}
				
			
			};
			return users;
        }
    }
}
