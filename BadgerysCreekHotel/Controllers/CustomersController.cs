using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using BadgerysCreekHotel.Data;
using BadgerysCreekHotel.Models;
using BadgerysCreekHotel.Models.ViewModels;

namespace BadgerysCreekHotel.Controllers
{
    //[Authorize(Roles = "Customers")]
    public class CustomersController : Controller
    {
        
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customer.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.Email == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Surname,GivenName,Postcode")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Email,Surname,GivenName,Postcode")] Customer customer)
        {
            if (id != customer.Email)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Email))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.Email == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var customer = await _context.Customer.FindAsync(id);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool CustomerExists(string id)
        {
            return _context.Customer.Any(e => e.Email == id);
        }

        //6.2 Search Room Link
        // GET: Customers//SearchRoom
        [Authorize(Roles = "customers")]
        public IActionResult SearchRoom()
        {
            ViewBag.RoomList = new SelectList(_context.Room, "ID", "BedCount");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "customers")]
        public async Task<IActionResult> SearchRoom(SearchRoom search)
        {
            var dateIn = new SqliteParameter("dayIn", search.CheckIn);
            var dateOut = new SqliteParameter("dayOut", search.CheckOut);
            var bedCount = new SqliteParameter("bed", search.BedCount);


            /*
            var searchRoom = _context.Room.FromSqlRaw("select * from [Room] inner join [Booking] on [Room].ID = [Booking].RoomID "
                + "where [Room].ID not in"
                + "select [Room].ID from [Room] inner join [Booking] on [Room].ID = [Booking].RoomID"
                + "where [Booking].CheckOut > @dayIn", dateIn)
                inner join [Booking] on [Room].ID = [Booking].RoomID
                */
            var searchRoom = _context.Room.FromSqlRaw("select [Room].ID, [Room].Level, [Room].BedCount, [Room].Price from [Room] inner join [Booking] on [Room].ID = [Booking].RoomID "
                  + "where [Room].BedCount = @bed "
                  + "and [Room].ID not in "
                  + "(select [Room].ID from [Room] inner join [Booking] on [Room].ID = [Booking].RoomID "
                  + "where [Booking].CheckIn between @dayIn and @dayOut or [Booking].CheckOut between @dayIn and @dayOut )", bedCount, dateIn, dateOut)
                .Select(ro => new Room {ID = ro.ID, Level = ro.Level, BedCount = ro.BedCount, Price = ro.Price}).Distinct();

            ViewBag.SearchRoom = await searchRoom.ToListAsync();
            return View(search);
        }

        //7.2 Cal Stat
        //GET:Customers/CalcStats
        [Authorize(Roles= "administrators")]
        public async Task<IActionResult> CalcStats()
        {
            // divide the customers into groups by PostCode
            var cusGroups = _context.Customer.GroupBy(m => m.Postcode);
            // divide the booking into groups by roomID
            var bookingGroups = _context.Booking.GroupBy(m => m.RoomID);

            // for each group, get its PostCode and the number of customers in this group
            var PCStats = cusGroups.Select(p => new ProStat { PostCode = p.Key, NumOfCustomer = p.Count() });
            // for each group, get its RoomID and the number of Booking in this group
            var RoomStats = bookingGroups.Select(b => new BookingStat { RoomID = b.Key, NumOfBooking = b.Count() });
            ViewBag.BookingList = await RoomStats.ToListAsync();

            // pass the list of GenreStatistic objects to view
            return View(await PCStats.ToListAsync());
        }


    }
}
