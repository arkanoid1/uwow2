﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Hazzik.Objects {
	public class Inventory : IInventory {
		protected Item[] _items;
		protected UpdateFields _slotStart;

		public Inventory(IContainer container, UpdateFields slotStart, uint slotsCount) {
			Container = container;
			MaxCount = slotsCount;
			_items = new Item[slotsCount];
			_slotStart = slotStart;
		}

		#region IInventory Members

		public IContainer Container { get; private set; }

		public uint MaxCount { get; private set; }

		public Item this[int slot] {
			get { return GetItem(slot); }
			set { SetItem(slot, value); }
		}

		public int GetAmount(int id) {
			return (int)this.Where(x => x.Entry == id).Sum(x => x.StackCount);
		}

		IEnumerator<Item> IEnumerable<Item>.GetEnumerator() {
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public void DestroyItem(int slot) {
			var item = GetItem(slot);
			item.Destroy();
			SetItem(slot, null);
		}

		public int FindFreeSlot() {
			for(var i = InventorySlot.BackpackStart; i < InventorySlot.BackpackEnd; i++) {
				if(null == this[(int)i]) {
					return (int)i;
				}
			}
			return -1;
		}

		#endregion

		public IEnumerator<Item> GetEnumerator() {
			foreach(Item item in _items) {
				if(null != item) {
					if(item is IContainer) {
						foreach(Item item2 in ((IContainer)item).Inventory) {
							yield return item2;
						}
					}
					yield return item;
				}
			}
		}

		public virtual void SetItem(int slot, Item item) {
			if(slot < 0 || slot >= MaxCount) {
				throw new ArgumentOutOfRangeException("slot");
			}
			((WorldObject)Container).SetUInt64(_slotStart + (slot * 2), item != null ? item.Guid : 0);
			if(null != item) {
				item.Contained = Container;
			}
			_items[slot] = item;
		}

		public virtual Item GetItem(int slot) {
			if(slot < 0 || slot >= MaxCount) {
				throw new ArgumentOutOfRangeException("slot");
			}
			return _items[slot];
		}
	}
}