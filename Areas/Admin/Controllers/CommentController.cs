using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyGameConsole.Models;

namespace QuanLyGameConsole.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentController : Controller
    {
        private readonly GameConsoleContext _context;

        public CommentController(GameConsoleContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductComments
        public async Task<IActionResult> Index()
        {
            var gameConsoleContext = _context.ProductComments.Include(p => p.Customer).Include(p => p.Product);
            return View(await gameConsoleContext.ToListAsync());
        }

        // GET: Admin/ProductComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productComment = await _context.ProductComments
                .Include(p => p.Customer)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (productComment == null)
            {
                return NotFound();
            }

            return View(productComment);
        }

        // GET: Admin/ProductComments/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            return View();
        }

        // POST: Admin/ProductComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,ProductId,CustomerId,Contents,CreatedAt")] ProductComment productComment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", productComment.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", productComment.ProductId);
            return View(productComment);
        }

        // GET: Admin/ProductComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productComment = await _context.ProductComments.FindAsync(id);
            if (productComment == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", productComment.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", productComment.ProductId);
            return View(productComment);
        }

        // POST: Admin/ProductComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentId,ProductId,CustomerId,Contents,CreatedAt")] ProductComment productComment)
        {
            if (id != productComment.CommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCommentExists(productComment.CommentId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", productComment.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", productComment.ProductId);
            return View(productComment);
        }

        // GET: Admin/ProductComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productComment = await _context.ProductComments
                .Include(p => p.Customer)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (productComment == null)
            {
                return NotFound();
            }

            return View(productComment);
        }

        // POST: Admin/ProductComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productComment = await _context.ProductComments.FindAsync(id);
            if (productComment != null)
            {
                _context.ProductComments.Remove(productComment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductCommentExists(int id)
        {
            return _context.ProductComments.Any(e => e.CommentId == id);
        }
        // GET: Admin/Comment/Reply/5
        public async Task<IActionResult> Reply(int? id)
        {
            if (id == null) return NotFound();

            var comment = await _context.ProductComments
                .Include(c => c.Customer)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.CommentId == id);

            if (comment == null) return NotFound();

            return View(comment);
        }

        // POST: Admin/Comment/Reply/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(int id, string reply)
        {
            var comment = await _context.ProductComments.FindAsync(id);
            if (comment == null) return NotFound();

            comment.Reply = reply;
            comment.ReplyAt = DateTime.Now;

            _context.Update(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
