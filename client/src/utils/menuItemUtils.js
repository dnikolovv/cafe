export function flattenMenuItems(items) {
  return items.reduce((current, nextItem) => {
    const existing = current.find(x => x.item.number === nextItem.number);

    if (existing) {
      existing.count += 1;
    } else {
      current.push({ count: 1, item: nextItem });
    }

    return current;
  }, []);
}
